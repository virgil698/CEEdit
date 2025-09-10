using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CEEdit.Core.Models.Common
{
    /// <summary>
    /// 扩展方法类
    /// </summary>
    public static class Extensions
    {
        #region 字符串扩展

        /// <summary>
        /// 判断字符串是否为空或null
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是否为空</returns>
        public static bool IsNullOrEmpty(this string? str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 判断字符串是否为空白或null
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是否为空白</returns>
        public static bool IsNullOrWhiteSpace(this string? str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 验证字符串是否为有效的ID格式
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是否为有效ID</returns>
        public static bool IsValidId(this string str)
        {
            if (str.IsNullOrEmpty())
                return false;

            return Regex.IsMatch(str, Constants.IdValidationPattern);
        }

        /// <summary>
        /// 验证字符串是否为有效的项目名称格式
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>是否为有效项目名称</returns>
        public static bool IsValidProjectName(this string str)
        {
            if (str.IsNullOrEmpty())
                return false;

            if (str.Length > Constants.MaxProjectNameLength)
                return false;

            return Regex.IsMatch(str, Constants.ProjectNameValidationPattern);
        }

        /// <summary>
        /// 转换为帕斯卡命名法
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>帕斯卡命名法字符串</returns>
        public static string ToPascalCase(this string str)
        {
            if (str.IsNullOrEmpty())
                return str;

            var words = str.Split(new[] { ' ', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
            var result = new System.Text.StringBuilder();

            foreach (var word in words)
            {
                if (word.Length > 0)
                {
                    result.Append(char.ToUpper(word[0]));
                    if (word.Length > 1)
                        result.Append(word.Substring(1).ToLower());
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// 转换为驼峰命名法
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>驼峰命名法字符串</returns>
        public static string ToCamelCase(this string str)
        {
            var pascalCase = str.ToPascalCase();
            if (pascalCase.IsNullOrEmpty())
                return pascalCase;

            return char.ToLower(pascalCase[0]) + pascalCase.Substring(1);
        }

        /// <summary>
        /// 转换为下划线分隔格式
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>下划线分隔格式字符串</returns>
        public static string ToSnakeCase(this string str)
        {
            if (str.IsNullOrEmpty())
                return str;

            var result = Regex.Replace(str, @"([A-Z])", "_$1").Trim('_').ToLower();
            return Regex.Replace(result, @"[\s\-]+", "_");
        }

        /// <summary>
        /// 限制字符串长度
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <param name="ellipsis">省略号</param>
        /// <returns>限制长度后的字符串</returns>
        public static string Truncate(this string str, int maxLength, string ellipsis = "...")
        {
            if (str.IsNullOrEmpty() || str.Length <= maxLength)
                return str;

            return str.Substring(0, maxLength - ellipsis.Length) + ellipsis;
        }

        #endregion

        #region 集合扩展

        /// <summary>
        /// 判断集合是否为空或null
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="collection">集合</param>
        /// <returns>是否为空</returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T>? collection)
        {
            return collection == null || collection.Count == 0;
        }

        /// <summary>
        /// 安全添加元素（避免添加null）
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="collection">集合</param>
        /// <param name="item">要添加的元素</param>
        public static void SafeAdd<T>(this ICollection<T> collection, T? item)
        {
            if (item != null)
                collection.Add(item);
        }

        /// <summary>
        /// 批量添加元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="collection">集合</param>
        /// <param name="items">要添加的元素集合</param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (items == null) return;

            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// 安全获取字典值
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dictionary">字典</param>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>值或默认值</returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, 
            TKey key, TValue defaultValue = default!) where TKey : notnull
        {
            return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
        }

        #endregion

        #region 文件/路径扩展

        /// <summary>
        /// 确保目录存在
        /// </summary>
        /// <param name="path">目录路径</param>
        public static void EnsureDirectoryExists(this string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// 获取相对路径
        /// </summary>
        /// <param name="fullPath">完整路径</param>
        /// <param name="basePath">基础路径</param>
        /// <returns>相对路径</returns>
        public static string GetRelativePath(this string fullPath, string basePath)
        {
            if (fullPath.IsNullOrEmpty() || basePath.IsNullOrEmpty())
                return fullPath;

            var fullUri = new Uri(fullPath);
            var baseUri = new Uri(basePath.EndsWith(Path.DirectorySeparatorChar.ToString()) 
                ? basePath : basePath + Path.DirectorySeparatorChar);

            var relativeUri = baseUri.MakeRelativeUri(fullUri);
            return Uri.UnescapeDataString(relativeUri.ToString()).Replace('/', Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// 获取安全的文件名
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>安全的文件名</returns>
        public static string ToSafeFileName(this string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var safeFileName = string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
            return safeFileName.Trim();
        }

        /// <summary>
        /// 获取文件大小的可读格式
        /// </summary>
        /// <param name="bytes">字节数</param>
        /// <returns>可读的文件大小</returns>
        public static string ToReadableFileSize(this long bytes)
        {
            const long KB = 1024;
            const long MB = KB * 1024;
            const long GB = MB * 1024;
            const long TB = GB * 1024;

            return bytes switch
            {
                >= TB => $"{bytes / (double)TB:F2} TB",
                >= GB => $"{bytes / (double)GB:F2} GB",
                >= MB => $"{bytes / (double)MB:F2} MB",
                >= KB => $"{bytes / (double)KB:F2} KB",
                _ => $"{bytes} B"
            };
        }

        #endregion

        #region 序列化扩展

        /// <summary>
        /// 将对象序列化为JSON字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="settings">序列化设置</param>
        /// <returns>JSON字符串</returns>
        public static string ToJson<T>(this T obj, JsonSerializerSettings? settings = null)
        {
            settings ??= new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// 从JSON字符串反序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">JSON字符串</param>
        /// <param name="settings">反序列化设置</param>
        /// <returns>对象</returns>
        public static T? FromJson<T>(this string json, JsonSerializerSettings? settings = null)
        {
            if (json.IsNullOrEmpty())
                return default;

            settings ??= new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        #endregion

        #region 时间扩展

        /// <summary>
        /// 转换为Unix时间戳
        /// </summary>
        /// <param name="dateTime">日期时间</param>
        /// <returns>Unix时间戳</returns>
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        }

        /// <summary>
        /// 从Unix时间戳转换为DateTime
        /// </summary>
        /// <param name="timestamp">Unix时间戳</param>
        /// <returns>DateTime</returns>
        public static DateTime FromUnixTimestamp(this long timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
        }

        /// <summary>
        /// 获取友好的时间差显示
        /// </summary>
        /// <param name="dateTime">日期时间</param>
        /// <returns>友好的时间差字符串</returns>
        public static string ToFriendlyTimeSpan(this DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            return timeSpan.TotalSeconds switch
            {
                < 60 => "刚刚",
                < 3600 => $"{(int)timeSpan.TotalMinutes} 分钟前",
                < 86400 => $"{(int)timeSpan.TotalHours} 小时前",
                < 2592000 => $"{(int)timeSpan.TotalDays} 天前",
                _ => dateTime.ToString("yyyy-MM-dd")
            };
        }

        #endregion

        #region 版本比较扩展

        /// <summary>
        /// 比较版本号
        /// </summary>
        /// <param name="version">当前版本</param>
        /// <param name="otherVersion">其他版本</param>
        /// <returns>比较结果</returns>
        public static int CompareVersion(this string version, string otherVersion)
        {
            if (Version.TryParse(version, out var v1) && Version.TryParse(otherVersion, out var v2))
            {
                return v1.CompareTo(v2);
            }

            return string.Compare(version, otherVersion, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 检查版本是否兼容
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="minVersion">最小版本</param>
        /// <param name="maxVersion">最大版本</param>
        /// <returns>是否兼容</returns>
        public static bool IsCompatibleVersion(this string version, string minVersion, string? maxVersion = null)
        {
            var compareMin = version.CompareVersion(minVersion);
            if (compareMin < 0)
                return false;

            if (maxVersion != null)
            {
                var compareMax = version.CompareVersion(maxVersion);
                if (compareMax > 0)
                    return false;
            }

            return true;
        }

        #endregion

        #region 枚举扩展

        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="enumValue">枚举值</param>
        /// <returns>描述</returns>
        public static string GetDescription(this System.Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var attributes = field?.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            
            if (attributes?.Length > 0 && attributes[0] is System.ComponentModel.DescriptionAttribute description)
                return description.Description;
            
            return enumValue.ToString();
        }

        #endregion
    }
}

