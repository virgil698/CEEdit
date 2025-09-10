using System.Collections.Generic;
using CEEdit.Core.Models.Common;

namespace CEEdit.Core.Models.Resources
{
    /// <summary>
    /// 资源包模型
    /// </summary>
    public class ResourcePack
    {
        /// <summary>
        /// 资源包名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 资源包描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 资源包版本
        /// </summary>
        public string Version { get; set; } = "1.0.0";

        /// <summary>
        /// 资源包格式版本
        /// </summary>
        public int PackFormat { get; set; } = 15;

        /// <summary>
        /// 纹理资源
        /// </summary>
        public List<Texture> Textures { get; set; } = new();

        /// <summary>
        /// 模型资源
        /// </summary>
        public List<Model3D> Models { get; set; } = new();

        /// <summary>
        /// 音效资源
        /// </summary>
        public List<AudioClip> Sounds { get; set; } = new();

        /// <summary>
        /// 语言文件
        /// </summary>
        public List<LanguageFile> Languages { get; set; } = new();

        /// <summary>
        /// 字体资源
        /// </summary>
        public List<FontResource> Fonts { get; set; } = new();

        /// <summary>
        /// 着色器资源
        /// </summary>
        public List<ShaderResource> Shaders { get; set; } = new();

        /// <summary>
        /// 资源包图标
        /// </summary>
        public string IconPath { get; set; } = string.Empty;

        /// <summary>
        /// 资源包根路径
        /// </summary>
        public string RootPath { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastModifiedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 资源统计信息
        /// </summary>
        public ResourceStatistics Statistics { get; set; } = new();

        /// <summary>
        /// 获取所有资源
        /// </summary>
        /// <returns>资源列表</returns>
        public List<ResourceBase> GetAllResources()
        {
            var resources = new List<ResourceBase>();
            resources.AddRange(Textures);
            resources.AddRange(Models);
            resources.AddRange(Sounds);
            resources.AddRange(Languages);
            resources.AddRange(Fonts);
            resources.AddRange(Shaders);
            return resources;
        }

        /// <summary>
        /// 根据类型获取资源
        /// </summary>
        /// <param name="type">资源类型</param>
        /// <returns>资源列表</returns>
        public List<ResourceBase> GetResourcesByType(ResourceType type)
        {
            return type switch
            {
                ResourceType.Texture => Textures.Cast<ResourceBase>().ToList(),
                ResourceType.Model => Models.Cast<ResourceBase>().ToList(),
                ResourceType.Sound => Sounds.Cast<ResourceBase>().ToList(),
                ResourceType.Language => Languages.Cast<ResourceBase>().ToList(),
                ResourceType.Font => Fonts.Cast<ResourceBase>().ToList(),
                ResourceType.Shader => Shaders.Cast<ResourceBase>().ToList(),
                _ => new List<ResourceBase>()
            };
        }

        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="resource">资源</param>
        public void AddResource(ResourceBase resource)
        {
            switch (resource)
            {
                case Texture texture:
                    Textures.Add(texture);
                    break;
                case Model3D model:
                    Models.Add(model);
                    break;
                case AudioClip sound:
                    Sounds.Add(sound);
                    break;
                case LanguageFile language:
                    Languages.Add(language);
                    break;
                case FontResource font:
                    Fonts.Add(font);
                    break;
                case ShaderResource shader:
                    Shaders.Add(shader);
                    break;
            }

            UpdateStatistics();
            LastModifiedAt = DateTime.Now;
        }

        /// <summary>
        /// 移除资源
        /// </summary>
        /// <param name="resource">资源</param>
        public void RemoveResource(ResourceBase resource)
        {
            switch (resource)
            {
                case Texture texture:
                    Textures.Remove(texture);
                    break;
                case Model3D model:
                    Models.Remove(model);
                    break;
                case AudioClip sound:
                    Sounds.Remove(sound);
                    break;
                case LanguageFile language:
                    Languages.Remove(language);
                    break;
                case FontResource font:
                    Fonts.Remove(font);
                    break;
                case ShaderResource shader:
                    Shaders.Remove(shader);
                    break;
            }

            UpdateStatistics();
            LastModifiedAt = DateTime.Now;
        }

        /// <summary>
        /// 更新统计信息
        /// </summary>
        private void UpdateStatistics()
        {
            Statistics.TextureCount = Textures.Count;
            Statistics.ModelCount = Models.Count;
            Statistics.SoundCount = Sounds.Count;
            Statistics.LanguageCount = Languages.Count;
            Statistics.FontCount = Fonts.Count;
            Statistics.ShaderCount = Shaders.Count;

            // 计算总文件大小
            var allResources = GetAllResources();
            Statistics.TotalFileSize = allResources.Sum(r => r.FileSize);
        }

        /// <summary>
        /// 验证资源包
        /// </summary>
        /// <returns>验证结果</returns>
        public ValidationResult Validate()
        {
            var result = new ValidationResult();

            // 验证基本信息
            if (string.IsNullOrEmpty(Name))
                result.AddError("资源包名称不能为空");

            if (PackFormat <= 0)
                result.AddError("资源包格式版本必须大于0");

            // 验证资源文件
            ValidateResources(result);

            return result;
        }

        /// <summary>
        /// 验证资源文件
        /// </summary>
        /// <param name="result">验证结果</param>
        private void ValidateResources(ValidationResult result)
        {
            var allResources = GetAllResources();
            var duplicateNames = allResources
                .GroupBy(r => r.Name)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key);

            foreach (var name in duplicateNames)
            {
                result.AddWarning($"发现重复的资源名称: {name}");
            }

            // 检查必需的资源
            if (!Textures.Any())
                result.AddWarning("资源包中没有纹理文件");

            // 验证文件路径
            foreach (var resource in allResources)
            {
                if (string.IsNullOrEmpty(resource.FilePath))
                    result.AddError($"资源 {resource.Name} 的文件路径为空");
            }
        }
    }

    /// <summary>
    /// 资源统计信息
    /// </summary>
    public class ResourceStatistics
    {
        /// <summary>
        /// 纹理数量
        /// </summary>
        public int TextureCount { get; set; }

        /// <summary>
        /// 模型数量
        /// </summary>
        public int ModelCount { get; set; }

        /// <summary>
        /// 音效数量
        /// </summary>
        public int SoundCount { get; set; }

        /// <summary>
        /// 语言数量
        /// </summary>
        public int LanguageCount { get; set; }

        /// <summary>
        /// 字体数量
        /// </summary>
        public int FontCount { get; set; }

        /// <summary>
        /// 着色器数量
        /// </summary>
        public int ShaderCount { get; set; }

        /// <summary>
        /// 总文件大小（字节）
        /// </summary>
        public long TotalFileSize { get; set; }

        /// <summary>
        /// 总资源数量
        /// </summary>
        public int TotalResourceCount => TextureCount + ModelCount + SoundCount + 
                                        LanguageCount + FontCount + ShaderCount;
    }

    /// <summary>
    /// 验证结果
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid => Errors.Count == 0;

        /// <summary>
        /// 错误列表
        /// </summary>
        public List<ValidationMessage> Errors { get; set; } = new();

        /// <summary>
        /// 警告列表
        /// </summary>
        public List<ValidationMessage> Warnings { get; set; } = new();

        /// <summary>
        /// 信息列表
        /// </summary>
        public List<ValidationMessage> Infos { get; set; } = new();

        /// <summary>
        /// 添加错误
        /// </summary>
        /// <param name="message">错误消息</param>
        public void AddError(string message)
        {
            Errors.Add(new ValidationMessage(ValidationLevel.Error, message));
        }

        /// <summary>
        /// 添加警告
        /// </summary>
        /// <param name="message">警告消息</param>
        public void AddWarning(string message)
        {
            Warnings.Add(new ValidationMessage(ValidationLevel.Warning, message));
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="message">信息消息</param>
        public void AddInfo(string message)
        {
            Infos.Add(new ValidationMessage(ValidationLevel.Info, message));
        }
    }

    /// <summary>
    /// 验证消息
    /// </summary>
    public class ValidationMessage
    {
        /// <summary>
        /// 消息级别
        /// </summary>
        public ValidationLevel Level { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="level">消息级别</param>
        /// <param name="message">消息内容</param>
        public ValidationMessage(ValidationLevel level, string message)
        {
            Level = level;
            Message = message;
            Timestamp = DateTime.Now;
        }
    }
}

