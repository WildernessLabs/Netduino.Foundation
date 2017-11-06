using System;
using System.IO;
using System.Collections;
using Microsoft.SPOT.IO;

namespace Netduino.Foundation.Configuration
{
    /// <summary>
    ///     Read an INI file from the SD card.
    /// </summary>
    /// <remarks>
    ///     Original posting in the Netduino forums:
    ///     http://forums.netduino.com/index.php?/topic/3680-reading-wriring-ini-file-from-sd-micro/
    ///     Original author: OneManDo.
    ///     All postings in the forums are shared under the Creative Commons 
    ///     Attribution-ShareAlike License.
    /// </remarks>
    public class IniFile
    {
        #region Member variables / fields

        /// <summary>
        ///     Hashtable of the sections in the INI file.
        /// </summary>
        private readonly Hashtable _sections;

        #endregion Member variables / fields

        #region Properties

        /// <summary>
        /// Name of the configuration (INI) file.
        /// </summary>
        public string FileName { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public IniFile()
        {
            _sections = new Hashtable();
        }

        /// <summary>
        ///     Create anew IniFile object and read the contents of the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file to read.</param>
        public IniFile(string fileName)
        {
            _sections = new Hashtable();
            Load(fileName, true);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     Loads the Reads the data in the ini file into the IniFile object
        /// </summary>
        /// <param name="filename">Name of the file containing the configuration.</param>
        /// <param name="append">
        ///     Append the contents of the file (if true), delete the old contents and create a new collection
        ///     (when false).
        /// </param>
        public void Load(string fileName, bool append)
        {
            if (!append)
            {
                DeleteAllSections();
            }
            FileName = fileName;

            var section = "";
            string[] keyValuePair;
            string actualFileName = Path.Combine("\\SD", fileName);
            using (FileStream fileStream = new FileStream(actualFileName, FileMode.Open))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    string[] lines = streamReader.ReadToEnd().ToLower().Split(new[] {'\n', '\r'});
                    foreach (var line in lines)
                    {
                        if (line == string.Empty)
                        {
                            continue;
                        }
                        if (';' == line[0])
                        {
                            continue;
                        }
                        if (('[' == line[0]) && (']' == line[line.Length - 1]))
                        {
                            section = line.Substring(1, line.Length - 2);
                        }
                        else if (-1 != line.IndexOf("="))
                        {
                            keyValuePair = line.Split('=');
                            SetValue(section, keyValuePair[0], keyValuePair[1]);
                        }
                    }
                    streamReader.Close();
                    fileStream.Close();
                }
            }
        }

        /// <summary>
        ///     Save the current sections/key/value combinations to a text file.
        /// </summary>
        /// <remarks>
        ///     The configuration will overwrite the original INI file.  A new file can be 
        ///     created by setting the FileName property before calling this method.
        /// </remarks>
        public void Save()
        {
            string actualFileName = Path.Combine("\\SD", FileName);
            using (FileStream fileStream = new FileStream(actualFileName, FileMode.Create))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    foreach (var section in _sections.Keys)
                    {
                        streamWriter.WriteLine("[" + section + "]");
                        var keyValuePair = (Hashtable) _sections[section];
                        foreach (var key in keyValuePair.Keys)
                        {
                            streamWriter.WriteLine(key + "=" + keyValuePair[key]);
                        }
                    }
                    streamWriter.Close();
                    fileStream.Close();
                    //
                    //  Ensure that the file is flushed to disk.
                    //
                    var volumes = VolumeInfo.GetVolumes();
                    foreach (var volume in volumes)
                    {
                        volume.FlushAll();
                    }
                }
            }
        }

        /// <summary>
        ///     Get a string from the hashtable.
        ///     Searches the hash table for the Section and key pair returning the associated value
        ///     (if the pair exists) or the default value if the pair does not exist.
        /// </summary>
        /// <param name="section">Name of the section holding the key.</param>
        /// <param name="key">Name of the key.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>
        ///     Value associated with the key in the specified section or the default value if the 
        ///     section/key/value combination does not exist.
        /// </returns>
        public string GetValue(string section, string key, string defaultValue = "")
        {
            key = key.ToLower();
            section = section.ToLower();
            var keyvalpair = (Hashtable) _sections[section];
            if ((null != keyvalpair) && (keyvalpair.Contains(key)))
            {
                defaultValue = keyvalpair[key].ToString();
            }
            return defaultValue;
        }

        /// <summary>
        ///     Get a floating point number from the hashtable.
        ///     Searches the hash table for the Section and key pair returning the associated value
        ///     (if the pair exists) or the default value if the pair does not exist.
        /// </summary>
        /// <param name="section">Name of the section holding the key.</param>
        /// <param name="key">Name of the key.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>
        ///     Value associated with the key in the specified section or the default value if the 
        ///     section/key/value combination does not exist.
        /// </returns>
        public float GetValue(string section, string key, float defaultValue)
        {
            try
            {
                defaultValue = (float) double.Parse(GetValue(section, key, defaultValue.ToString()));
            }
            catch (Exception)
            {
            }
            return defaultValue;
        }

        /// <summary>
        ///     Get a double from the hashtable.
        ///     Searches the hash table for the Section and key pair returning the associated value
        ///     (if the pair exists) or the default value if the pair does not exist.
        /// </summary>
        /// <param name="section">Name of the section holding the key.</param>
        /// <param name="key">Name of the key.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>
        ///     Value associated with the key in the specified section or the default value if the section/key/value
        ///     combination does not exist.
        /// </returns>
        public double GetValue(string section, string key, double defaultValue)
        {
            try
            {
                defaultValue = double.Parse(GetValue(section, key, defaultValue.ToString()));
            }
            catch (Exception)
            {
            }
            return defaultValue;
        }

        /// <summary>
        ///     Get a UInt64 from the hashtable.
        ///     Searches the hash table for the Section and key pair returning the associated value
        ///     (if the pair exists) or the default value if the pair does not exist.
        /// </summary>
        /// <param name="section">Name of the section holding the key.</param>
        /// <param name="key">Name of the key.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>
        ///     Value associated with the key in the specified section or the default value if the 
        ///     section/key/value combination does not exist.
        /// </returns>
        public ulong GetValue(string section, string key, ulong defaultValue)
        {
            try
            {
                defaultValue = ulong.Parse(GetValue(section, key, defaultValue.ToString()));
            }
            catch (Exception)
            {
            }
            return defaultValue;
        }

        /// <summary>
        ///     Get a UInt32 from the hashtable.
        ///     Searches the hash table for the Section and key pair returning the associated value
        ///     (if the pair exists) or the default value if the pair does not exist.
        /// </summary>
        /// <param name="section">Name of the section holding the key.</param>
        /// <param name="key">Name of the key.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>
        ///     Value associated with the key in the specified section or the default value if the 
        ///     section/key/value combination does not exist.
        /// </returns>
        public uint GetValue(string section, string key, uint defaultValue)
        {
            try
            {
                defaultValue = uint.Parse(GetValue(section, key, defaultValue.ToString()));
            }
            catch (Exception)
            {
            }
            return defaultValue;
        }

        /// <summary>
        ///     Get a UInt16 from the hashtable.
        ///     Searches the hash table for the Section and key pair returning the associated value
        ///     (if the pair exists) or the default value if the pair does not exist.
        /// </summary>
        /// <param name="section">Name of the section holding the key.</param>
        /// <param name="key">Name of the key.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>
        ///     Value associated with the key in the specified section or the default value if the 
        ///     section/key/value combination does not exist.
        /// </returns>
        public ushort GetValue(string section, string key, ushort defaultValue)
        {
            try
            {
                defaultValue = ushort.Parse(GetValue(section, key, defaultValue.ToString()));
            }
            catch (Exception)
            {
            }
            return defaultValue;
        }

        /// <summary>
        ///     Get a byte from the hashtable.
        ///     Searches the hash table for the Section and key pair returning the associated value
        ///     (if the pair exists) or the default value if the pair does not exist.
        /// </summary>
        /// <param name="section">Name of the section holding the key.</param>
        /// <param name="key">Name of the key.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>
        ///     Value associated with the key in the specified section or the default value if the 
        ///     section/key/value combination does not exist.
        /// </returns>
        public byte GetValue(string section, string key, byte defaultValue)
        {
            try
            {
                defaultValue = byte.Parse(GetValue(section, key, defaultValue.ToString()));
            }
            catch (Exception)
            {
            }
            return defaultValue;
        }

        /// <summary>
        ///     Get a Int64 from the hashtable.
        ///     Searches the hash table for the Section and key pair returning the associated value
        ///     (if the pair exists) or the default value if the pair does not exist.
        /// </summary>
        /// <param name="section">Name of the section holding the key.</param>
        /// <param name="key">Name of the key.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>
        ///     Value associated with the key in the specified section or the default value if the 
        ///     section/key/value combination does not exist.
        /// </returns>
        public long GetValue(string section, string key, long defaultValue)
        {
            try
            {
                defaultValue = long.Parse(GetValue(section, key, defaultValue.ToString()));
            }
            catch (Exception)
            {
            }
            return defaultValue;
        }

        /// <summary>
        ///     Get a Int32 from the hashtable.
        ///     Searches the hash table for the Section and key pair returning the associated value
        ///     (if the pair exists) or the default value if the pair does not exist.
        /// </summary>
        /// <param name="section">Name of the section holding the key.</param>
        /// <param name="key">Name of the key.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>
        ///     Value associated with the key in the specified section or the default value if the 
        ///     section/key/value combination does not exist.
        /// </returns>
        public int GetValue(string section, string key, int defaultValue)
        {
            try
            {
                defaultValue = int.Parse(GetValue(section, key, defaultValue.ToString()));
            }
            catch (Exception)
            {
            }
            return defaultValue;
        }

        /// <summary>
        ///     Get a double from the hashtable.
        ///     Searches the hash table for the Section and key pair returning the associated value
        ///     (if the pair exists) or the default value if the pair does not exist.
        /// </summary>
        /// <param name="section">Name of the section holding the key.</param>
        /// <param name="key">Name of the key.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>
        ///     Value associated with the key in the specified section or the default value if the 
        ///     section/key/value combination does not exist.
        /// </returns>
        public short GetValue(string section, string key, short defaultValue)
        {
            try
            {
                defaultValue = short.Parse(GetValue(section, key, defaultValue.ToString()));
            }
            catch (Exception)
            {
            }
            return defaultValue;
        }

        /// <summary>
        ///     Set the value for the key in the specified section.
        /// </summary>
        /// <param name="section">Name of the section to hold the key.</param>
        /// <param name="key">Name of the key to add.</param>
        /// <param name="value">Value to add.</param>
        public void SetValue(string section, string key, string value)
        {
            key = key.ToLower();
            section = section.ToLower();
            if (!_sections.Contains(section))
            {
                _sections.Add(section, new Hashtable());
            }
            var keyvalpair = (Hashtable) _sections[section];
            if (keyvalpair.Contains(key))
            {
                keyvalpair[key] = value;
            }
            else
            {
                keyvalpair.Add(key, value);
            }
        }

        /// <summary>
        ///     Set the value for the key in the specified section.
        /// </summary>
        /// <param name="section">Name of the section to hold the key.</param>
        /// <param name="key">Name of the key to add.</param>
        /// <param name="value">Value to add.</param>
        public void SetValue(string section, string key, float value)
        {
            SetValue(section, key, value.ToString());
        }

        /// <summary>
        ///     Set the value for the key in the specified section.
        /// </summary>
        /// <param name="section">Name of the section to hold the key.</param>
        /// <param name="key">Name of the key to add.</param>
        /// <param name="value">Value to add.</param>
        public void SetValue(string section, string key, double value)
        {
            SetValue(section, key, value.ToString());
        }

        /// <summary>
        ///     Set the value for the key in the specified section.
        /// </summary>
        /// <param name="section">Name of the section to hold the key.</param>
        /// <param name="key">Name of the key to add.</param>
        /// <param name="value">Value to add.</param>
        public void SetValue(string section, string key, byte value)
        {
            SetValue(section, key, value.ToString());
        }

        /// <summary>
        ///     Set the value for the key in the specified section.
        /// </summary>
        /// <param name="section">Name of the section to hold the key.</param>
        /// <param name="key">Name of the key to add.</param>
        /// <param name="value">Value to add.</param>
        public void SetValue(string section, string key, short value)
        {
            SetValue(section, key, value.ToString());
        }

        /// <summary>
        ///     Set the value for the key in the specified section.
        /// </summary>
        /// <param name="section">Name of the section to hold the key.</param>
        /// <param name="key">Name of the key to add.</param>
        /// <param name="value">Value to add.</param>
        public void SetValue(string section, string key, int value)
        {
            SetValue(section, key, value.ToString());
        }

        /// <summary>
        ///     Set the value for the key in the specified section.
        /// </summary>
        /// <param name="section">Name of the section to hold the key.</param>
        /// <param name="key">Name of the key to add.</param>
        /// <param name="value">Value to add.</param>
        public void SetValue(string section, string key, long value)
        {
            SetValue(section, key, value.ToString());
        }

        /// <summary>
        ///     Set the value for the key in the specified section.
        /// </summary>
        /// <param name="section">Name of the section to hold the key.</param>
        /// <param name="key">Name of the key to add.</param>
        /// <param name="value">Value to add.</param>
        public void SetValue(string section, string key, char value)
        {
            SetValue(section, key, value.ToString());
        }

        /// <summary>
        ///     Set the value for the key in the specified section.
        /// </summary>
        /// <param name="section">Name of the section to hold the key.</param>
        /// <param name="key">Name of the key to add.</param>
        /// <param name="value">Value to add.</param>
        public void SetValue(string section, string key, ushort value)
        {
            SetValue(section, key, value.ToString());
        }

        /// <summary>
        ///     Set the value for the key in the specified section.
        /// </summary>
        /// <param name="section">Name of the section to hold the key.</param>
        /// <param name="key">Name of the key to add.</param>
        /// <param name="value">Value to add.</param>
        public void SetValue(string section, string key, uint value)
        {
            SetValue(section, key, value.ToString());
        }

        /// <summary>
        ///     Set the value for the key in the specified section.
        /// </summary>
        /// <param name="section">Name of the section to hold the key.</param>
        /// <param name="key">Name of the key to add.</param>
        /// <param name="value">Value to add.</param>
        public void SetValue(string section, string key, ulong value)
        {
            SetValue(section, key, value.ToString());
        }

        /// <summary>
        ///     Delte a single section from the hashtable.
        /// </summary>
        /// <remarks>
        ///     Passing a section name that does not exist in the hashtable will have no effect.
        /// </remarks>
        /// <param name="section">Name of the section to delete.</param>
        public void DeleteSection(string section)
        {
            section = section.ToLower();
            if (_sections.Contains(section))
            {
                _sections.Remove(section);
            }
        }

        /// <summary>
        ///     Delete all of the sections in the hashtable.
        /// </summary>
        public void DeleteAllSections()
        {
            _sections.Clear();
        }

        #endregion Methods
    }
}