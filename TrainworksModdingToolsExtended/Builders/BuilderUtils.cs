using Trainworks.Managers;

namespace Trainworks.BuildersV2
{
    public class BuilderUtils
    {
        /// <summary>
        /// Imports localization data for a key.
        /// Sets the translation to text for all languages.
        /// If either key or text is null, the function returns harmlessly.
        /// </summary>
        /// <param name="key">Key to set localization data for</param>
        /// <param name="text">Text for the key in all languages</param>
        public static void ImportStandardLocalization(string key, string text)
        {
            if (key == "")
            {
                return;
            }

            if (key == null || text == null)
            {
                return;
            }

            CustomLocalizationManager.ImportSingleLocalization(key, "Text", "", "", "", "", text, text, text, text, text, text);
        }
    }
}