using UnityEngine;
using QFramework;
using Cysharp.Threading.Tasks;

public static class ImagePickerExtensions
{
    /// <summary>
    /// 快速选择图片并设置到Image组件
    /// </summary>
    public static async UniTask<bool> PickAndSetImageAsync(this IController controller, UnityEngine.UI.Image targetImage, int maxSize = 1024)
    {
        if (targetImage == null)
        {
            Debug.LogError("目标Image组件为空");
            return false;
        }
        
        var imageUtility = controller.GetUtility<IImagePickerUtility>();
        var texture = await imageUtility.PickImageFromGalleryAsync(maxSize);
        
        if (texture != null)
        {
            // 创建Sprite并设置到Image
            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );
            
            targetImage.sprite = sprite;
            targetImage.preserveAspect = true;
            
            Debug.Log($"图片设置成功: {texture.width}x{texture.height}");
            return true;
        }
        
        return false;
    }
    
    /// <summary>
    /// 快速选择图片并保存到缓存
    /// </summary>
    public static async UniTask<string> PickAndSaveToCacheAsync(this IController controller, string fileName, int maxSize = 1024)
    {
        var imageUtility = controller.GetUtility<IImagePickerUtility>();
        
        // 先选择图片
        var texture = await imageUtility.PickImageFromGalleryAsync(maxSize);
        if (texture == null)
        {
            Debug.Log("未选择图片");
            return null;
        }
        
        // 保存到缓存
        string savedPath = await imageUtility.SaveImageToCacheAsync(texture, fileName);
        
        // 清理临时纹理
        UnityEngine.Object.Destroy(texture);
        
        return savedPath;
    }
    
    /// <summary>
    /// 从缓存加载图片并设置到Image组件
    /// </summary>
    public static async UniTask<bool> LoadFromCacheAndSetAsync(this IController controller, UnityEngine.UI.Image targetImage, string fileName)
    {
        if (targetImage == null)
        {
            Debug.LogError("目标Image组件为空");
            return false;
        }
        
        var imageUtility = controller.GetUtility<IImagePickerUtility>();
        
        // 检查缓存是否存在
        if (!imageUtility.IsImageCached(fileName))
        {
            Debug.LogWarning($"缓存中不存在图片: {fileName}");
            return false;
        }
        
        // 从缓存加载
        var texture = await imageUtility.LoadImageFromCacheAsync(fileName);
        
        if (texture != null)
        {
            // 创建Sprite并设置到Image
            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );
            
            targetImage.sprite = sprite;
            targetImage.preserveAspect = true;
            
            Debug.Log($"从缓存加载图片成功: {fileName} ({texture.width}x{texture.height})");
            return true;
        }
        
        return false;
    }
    
    /// <summary>
    /// 检查图片是否已缓存
    /// </summary>
    public static bool IsImageCached(this IController controller, string fileName)
    {
        var imageUtility = controller.GetUtility<IImagePickerUtility>();
        return imageUtility.IsImageCached(fileName);
    }
    
    /// <summary>
    /// 删除缓存图片
    /// </summary>
    public static bool DeleteCachedImage(this IController controller, string fileName)
    {
        var imageUtility = controller.GetUtility<IImagePickerUtility>();
        return imageUtility.DeleteCachedImage(fileName);
    }
    
    /// <summary>
    /// 清空所有图片缓存
    /// </summary>
    public static void ClearImageCache(this IController controller)
    {
        var imageUtility = controller.GetUtility<IImagePickerUtility>();
        imageUtility.ClearImageCache();
    }
} 