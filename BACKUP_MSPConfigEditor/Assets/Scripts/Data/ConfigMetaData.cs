using System;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

[Serializable]
public class ConfigMetaData
{
    public string date_modified;
    public string data_model_hash;
    public int errors;
    public float editor_version;
	public string min_supported_client;
	public string max_supported_client;

	public ConfigMetaData(string a_dataModelJSON, string a_minSupportedClient = "", string a_maxSupportedClient = "")
    {
        date_modified = DateTime.Now.ToShortDateString();
        data_model_hash = GetStringSha256Hash(a_dataModelJSON);
        errors = DrawerManager.Instance.ConstraintWindow.Errors;
        editor_version = DrawerManager.Instance.DrawerOptions.EditorVersion;
		min_supported_client = a_minSupportedClient;
		max_supported_client = a_maxSupportedClient;
	}

    string CalculateMD5(object a_obj)
    {
        if (a_obj == null)
            return null;
        using (var md5 = MD5.Create())
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, a_obj);
                return Encoding.Default.GetString(md5.ComputeHash(ms));
            }
        }
    }

    internal static string GetStringSha256Hash(string text)
    {
        if (String.IsNullOrEmpty(text))
            return String.Empty;

        using (var sha = new SHA256Managed())
        {
            byte[] textData = Encoding.UTF8.GetBytes(text);
            byte[] hash = sha.ComputeHash(textData);
            return BitConverter.ToString(hash).Replace("-", String.Empty);
        }
    }
}

