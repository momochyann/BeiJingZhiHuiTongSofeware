using QFramework;

public class AppArchitecture : Architecture<AppArchitecture>
{
    protected override void Init()
    {
        // 注册图片选择工具
        this.RegisterUtility<IImagePickerUtility>(new ImagePickerUtility());
        
        // 其他工具的注册...
    }
} 