using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Cysharp.Threading.Tasks;

public class ImagePickerPanel : MonoBehaviour, IController
{
    [SerializeField] private Image imageDisplay;          // 图片显示框
    
    private Texture2D currentTexture;
    private const string IMAGE_NAME_PREFIX = "user_image_";
    
     void Start()
    {
        InitializeUI();
        LoadLastImage();
    }
    
    private void InitializeUI()
    {
        // 设置图片点击事件
        if (imageDisplay != null)
        {
            Button imageButton = imageDisplay.GetComponent<Button>();
            if (imageButton == null)
            {
                imageButton = imageDisplay.gameObject.AddComponent<Button>();
            }
            imageButton.onClick.AddListener(() => OnImageClicked().Forget());
        }
    }
    
    /// <summary>
    /// 图片被点击时的处理 - 直接打开相册
    /// </summary>
    private async UniTaskVoid OnImageClicked()
    {
        try
        {
            Debug.Log("打开相册选择图片");
            
            var imageUtility = this.GetUtility<ImagePickerUtility>();
            
            var newTexture = await imageUtility.PickImageFromGalleryAsync(1024);
            
            if (newTexture != null)
            {
                // 生成基于时间戳的文件名
                string fileName = GenerateImageFileName();
                
                // 自动保存到缓存
                string savedPath = await imageUtility.SaveImageToCacheAsync(newTexture, fileName);
                
                if (!string.IsNullOrEmpty(savedPath))
                {
                    // 显示新图片
                    SetDisplayImage(newTexture);
                    Debug.Log($"图片已保存并显示: {fileName}");
                    Debug.Log($"缓存路径: {savedPath}");
                }
                else
                {
                    // 保存失败，清理纹理
                    UnityEngine.Object.Destroy(newTexture);
                    Debug.LogWarning("图片保存失败");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"图片选择失败: {e.Message}");
        }
    }
    
    /// <summary>
    /// 启动时加载上次显示的图片
    /// </summary>
    private async void LoadLastImage()
    {
        try
        {
            var imageUtility = this.GetUtility<ImagePickerUtility>();
            string lastImageName = imageUtility.GetLastImageRecord();
            
            if (!string.IsNullOrEmpty(lastImageName) && imageUtility.IsImageCached(lastImageName))
            {
                var texture = await imageUtility.LoadImageFromCacheAsync(lastImageName);
                if (texture != null)
                {
                    SetDisplayImage(texture);
                    Debug.Log($"自动加载上次图片: {lastImageName}");
                    Debug.Log($"图片路径: {imageUtility.GetCachedImagePath(lastImageName)}");
                }
            }
            else
            {
                Debug.Log("没有找到上次的图片，显示默认状态");
                SetDefaultImage();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"加载上次图片失败: {e.Message}");
            SetDefaultImage();
        }
    }
    
    /// <summary>
    /// 设置显示的图片
    /// </summary>
    private void SetDisplayImage(Texture2D texture)
    {
        if (imageDisplay == null || texture == null) return;
        
        // 清理之前的纹理
        if (currentTexture != null)
        {
            UnityEngine.Object.Destroy(currentTexture);
        }
        
        currentTexture = texture;
        
        // 创建Sprite并显示
        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );
        
        imageDisplay.sprite = sprite;
        imageDisplay.preserveAspect = true;
        imageDisplay.color = Color.white; // 确保显示正常颜色
        
        Debug.Log($"图片显示成功: {texture.width}x{texture.height}");
    }
    
    /// <summary>
    /// 设置默认图片显示
    /// </summary>
    private void SetDefaultImage()
    {
        if (imageDisplay != null)
        {
            imageDisplay.sprite = null;
            imageDisplay.color = new Color(0.8f, 0.8f, 0.8f, 1f); // 浅灰色背景
        }
    }
    
    /// <summary>
    /// 生成基于时间戳的图片文件名
    /// </summary>
    private string GenerateImageFileName()
    {
        return $"{IMAGE_NAME_PREFIX}{System.DateTime.Now:yyyyMMdd_HHmmss}";
    }
    
    /// <summary>
    /// 获取当前显示图片的缓存路径（供其他地方使用）
    /// </summary>
    public string GetCurrentImageCachePath()
    {
        var imageUtility = this.GetUtility<ImagePickerUtility>();
        string lastImageName = imageUtility.GetLastImageRecord();
        
        if (!string.IsNullOrEmpty(lastImageName))
        {
            return imageUtility.GetCachedImagePath(lastImageName);
        }
        
        return null;
    }
    
    private void OnDestroy()
    {
        // 清理纹理资源
        if (currentTexture != null)
        {
            UnityEngine.Object.Destroy(currentTexture);
            currentTexture = null;
        }
    }

    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
} 