using UnityEngine;
using QFramework;
using Cysharp.Threading.Tasks;
using System;
using System.IO;
using NativeGalleryNamespace;

public interface IImagePickerUtility : IUtility
{
    /// <summary>
    /// 从相册选择图片
    /// </summary>
    /// <param name="maxSize">图片最大尺寸，-1表示不限制</param>
    UniTask<Texture2D> PickImageFromGalleryAsync(int maxSize = 1024);
    
    /// <summary>
    /// 保存图片到缓存目录
    /// </summary>
    /// <param name="texture">要保存的纹理</param>
    /// <param name="fileName">文件名（不含扩展名）</param>
    /// <returns>保存的文件路径</returns>
    UniTask<string> SaveImageToCacheAsync(Texture2D texture, string fileName);
    
    /// <summary>
    /// 从缓存目录加载图片
    /// </summary>
    /// <param name="fileName">文件名（不含扩展名）</param>
    UniTask<Texture2D> LoadImageFromCacheAsync(string fileName);
    
    /// <summary>
    /// 检查缓存中是否存在指定图片
    /// </summary>
    /// <param name="fileName">文件名（不含扩展名）</param>
    bool IsImageCached(string fileName);
    
    /// <summary>
    /// 删除缓存中的图片
    /// </summary>
    /// <param name="fileName">文件名（不含扩展名）</param>
    bool DeleteCachedImage(string fileName);
    
    /// <summary>
    /// 清空图片缓存
    /// </summary>
    void ClearImageCache();
    
    /// <summary>
    /// 保存当前显示图片的记录
    /// </summary>
    /// <param name="fileName">当前显示的图片文件名</param>
    void SaveCurrentImageRecord(string fileName);
    
    /// <summary>
    /// 获取上次显示的图片记录
    /// </summary>
    /// <returns>图片文件名，如果没有记录返回null</returns>
    string GetLastImageRecord();
    
    /// <summary>
    /// 获取缓存图片的完整路径
    /// </summary>
    /// <param name="fileName">文件名（不含扩展名）</param>
    /// <returns>完整的文件路径</returns>
    string GetCachedImagePath(string fileName);
}

public class ImagePickerUtility : IImagePickerUtility
{
    private string ImageCacheDirectory => Path.Combine(Application.persistentDataPath, "ImageCache");
    private const string LAST_IMAGE_RECORD_KEY = "LastDisplayedImage";
    
    /// <summary>
    /// 从相册选择图片
    /// </summary>
    public async UniTask<Texture2D> PickImageFromGalleryAsync(int maxSize = 1024)
    {
        try
        {
            Debug.Log("开始从相册选择图片...");
            
            // 编辑器模式直接返回测试纹理
            if (Application.isEditor)
            {
                return CreateTestTexture();
            }
            
            // 检查权限
            if (!await RequestGalleryPermissionAsync())
            {
                Debug.LogError("没有相册访问权限");
                return null;
            }

            // 使用NativeGallery选择图片
            var tcs = new UniTaskCompletionSource<Texture2D>();
            
            NativeGallery.GetImageFromGallery((path) =>
            {
                if (string.IsNullOrEmpty(path))
                {
                    Debug.Log("用户取消选择图片");
                    tcs.TrySetResult(null);
                    return;
                }
                
                // 加载选择的图片
                try
                {
                    Debug.Log($"加载图片: {path}");
                    Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                    
                    if (texture != null)
                    {
                        Debug.Log($"图片加载成功: {texture.width}x{texture.height}");
                        tcs.TrySetResult(texture);
                    }
                    else
                    {
                        Debug.LogError("图片加载失败");
                        tcs.TrySetResult(null);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"加载图片异常: {e.Message}");
                    tcs.TrySetResult(null);
                }
            }, "选择图片", "image/*");
            
            return await tcs.Task;
        }
        catch (Exception e)
        {
            Debug.LogError($"选择图片失败: {e.Message}");
            return null;
        }
    }
    
    /// <summary>
    /// 保存图片到缓存目录
    /// </summary>
    public async UniTask<string> SaveImageToCacheAsync(Texture2D texture, string fileName)
    {
        if (texture == null)
        {
            Debug.LogError("纹理为空，无法保存");
            return null;
        }
        
        try
        {
            // 确保缓存目录存在
            EnsureCacheDirectoryExists();
            
            // 构建完整路径
            string filePath = Path.Combine(ImageCacheDirectory, fileName + ".png");
            
            // 将纹理转换为PNG字节数组
            byte[] imageData = texture.EncodeToPNG();
            
            // 异步写入文件
            await File.WriteAllBytesAsync(filePath, imageData);
            
            // 保存当前图片记录
            SaveCurrentImageRecord(fileName);
            
            Debug.Log($"图片已保存到缓存: {filePath}");
            return filePath;
        }
        catch (Exception e)
        {
            Debug.LogError($"保存图片到缓存失败: {e.Message}");
            return null;
        }
    }
    
    /// <summary>
    /// 从缓存目录加载图片
    /// </summary>
    public async UniTask<Texture2D> LoadImageFromCacheAsync(string fileName)
    {
        try
        {
            string filePath = Path.Combine(ImageCacheDirectory, fileName + ".png");
            
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"缓存中不存在图片: {fileName}");
                return null;
            }
            
            // 异步读取文件
            byte[] imageData = await File.ReadAllBytesAsync(filePath);
            
            // 创建纹理并加载图片数据
            Texture2D texture = new Texture2D(2, 2);
            
            if (texture.LoadImage(imageData))
            {
                Debug.Log($"从缓存加载图片成功: {fileName} ({texture.width}x{texture.height})");
                return texture;
            }
            else
            {
                Debug.LogError($"加载缓存图片失败: {fileName}");
                UnityEngine.Object.Destroy(texture);
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"从缓存加载图片异常: {e.Message}");
            return null;
        }
    }
    
    /// <summary>
    /// 检查缓存中是否存在指定图片
    /// </summary>
    public bool IsImageCached(string fileName)
    {
        string filePath = Path.Combine(ImageCacheDirectory, fileName + ".png");
        return File.Exists(filePath);
    }
    
    /// <summary>
    /// 删除缓存中的图片
    /// </summary>
    public bool DeleteCachedImage(string fileName)
    {
        try
        {
            string filePath = Path.Combine(ImageCacheDirectory, fileName + ".png");
            
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log($"已删除缓存图片: {fileName}");
                return true;
            }
            else
            {
                Debug.LogWarning($"要删除的缓存图片不存在: {fileName}");
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"删除缓存图片失败: {e.Message}");
            return false;
        }
    }
    
    /// <summary>
    /// 清空图片缓存
    /// </summary>
    public void ClearImageCache()
    {
        try
        {
            if (Directory.Exists(ImageCacheDirectory))
            {
                Directory.Delete(ImageCacheDirectory, true);
                Debug.Log("图片缓存已清空");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"清空图片缓存失败: {e.Message}");
        }
    }
    
    /// <summary>
    /// 保存当前显示图片的记录
    /// </summary>
    public void SaveCurrentImageRecord(string fileName)
    {
        PlayerPrefs.SetString(LAST_IMAGE_RECORD_KEY, fileName);
        PlayerPrefs.Save();
        Debug.Log($"保存图片记录: {fileName}");
    }
    
    /// <summary>
    /// 获取上次显示的图片记录
    /// </summary>
    public string GetLastImageRecord()
    {
        string lastImage = PlayerPrefs.GetString(LAST_IMAGE_RECORD_KEY, "");
        if (string.IsNullOrEmpty(lastImage))
        {
            Debug.Log("没有找到上次显示的图片记录");
            return null;
        }
        
        Debug.Log($"获取上次图片记录: {lastImage}");
        return lastImage;
    }
    
    /// <summary>
    /// 获取缓存图片的完整路径
    /// </summary>
    public string GetCachedImagePath(string fileName)
    {
        return Path.Combine(ImageCacheDirectory, fileName + ".png");
    }
    
    /// <summary>
    /// 确保缓存目录存在
    /// </summary>
    private void EnsureCacheDirectoryExists()
    {
        if (!Directory.Exists(ImageCacheDirectory))
        {
            Directory.CreateDirectory(ImageCacheDirectory);
            Debug.Log($"创建图片缓存目录: {ImageCacheDirectory}");
        }
    }
    
    /// <summary>
    /// 请求相册访问权限
    /// </summary>
    private async UniTask<bool> RequestGalleryPermissionAsync()
    {
        // 编辑器模式直接返回true
        if (Application.isEditor)
        {
            return true;
        }
        
        try
        {
            // 检查当前权限状态
            if (NativeGallery.CheckPermission(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image))
            {
                Debug.Log("相册权限已获得");
                return true;
            }
            
            // 请求权限 - 使用Task版本的API
            var permission = await NativeGallery.RequestPermissionAsync(NativeGallery.PermissionType.Read, NativeGallery.MediaType.Image);
            
            bool granted = permission == NativeGallery.Permission.Granted;
            Debug.Log($"相册权限请求结果: {permission} ({granted})");
            
            return granted;
        }
        catch (Exception e)
        {
            Debug.LogError($"检查相册权限失败: {e.Message}");
            return false;
        }
    }
    
    /// <summary>
    /// 创建测试纹理（编辑器模式使用）
    /// </summary>
    private Texture2D CreateTestTexture()
    {
        Texture2D testTexture = new Texture2D(256, 256);
        
        for (int x = 0; x < 256; x++)
        {
            for (int y = 0; y < 256; y++)
            {
                float r = (float)x / 256f;
                float g = (float)y / 256f;
                float b = 0.5f;
                testTexture.SetPixel(x, y, new Color(r, g, b, 1f));
            }
        }
        
        testTexture.Apply();
        Debug.Log("创建测试纹理成功");
        return testTexture;
    }
} 