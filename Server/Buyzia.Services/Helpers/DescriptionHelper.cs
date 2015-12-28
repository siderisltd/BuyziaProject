namespace Buyzia.Services.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using Buyzia.Data.Models;
    using Common;
    using Contracts;
    using ServiceStack.Html;
    using WebMarkupMin.Core.Minifiers;

    //TODO: Refactor and set item.OriginalName to item.SellingName
    public class DescriptionHelper
    {
        private const int RandomWordsToGet = 6;
        private const string KeyKeywords = "keywordswithcommasgoeshere";
        private const string KeyAltTitle = "titlegoeshere";
        private const string KeyMaintitle = "maintitle";
        private const string KeyImageLink = "imagelinkgoeshere";
        private const string KeyItemDescription = "itemdesc";
        private const string KeyBullets = "keybulleti";
        private static readonly List<string> constantKeyWords = "Buyzia, Eshop, Discount, Wholesale, Ebay, Best, Top".Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        private Item item;
        private string htmlTemplate;
        private List<string> keyWordsFromName;
        private List<string> randomKeyWords;
        private IItemService itemsService;

        public DescriptionHelper(Item item, IItemService itemsService)
        {
            this.item = item;
            this.itemsService = itemsService;
            //TODO: Fix keywords
      
            this.keyWordsFromName = this.GetKeywordsFromItemName(this.item.OriginalName);
            this.randomKeyWords = this.GetRandomKeyWordsFromItemFeatures(this.item.Features.Select(x => x.Content).ToList(), RandomWordsToGet);
            this.KeyWords = this.GatherAllKeyWords(constantKeyWords, this.keyWordsFromName, this.randomKeyWords);
        }

        public List<string> KeyWords { get; private set; }

        public string GetHtmlDescription()
        {

            var templ = this.LoadTemplate(HttpContext.Current.Server.MapPath("/" + Constants.DESCRIPTION_TEMPLATE_FILE_PATH)).Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).ToList();
            var sb = new StringBuilder();
            foreach (var htmlParagraph in templ)
            {
                sb.Append(htmlParagraph);
            }
            this.htmlTemplate = sb.ToString();

            string renderedHtml = this.RenderItemInformation(this.htmlTemplate);

            return renderedHtml;
        }

        /// <summary>
        /// Loads the html template
        /// </summary>
        /// <param name="path">html template file path</param>
        internal string LoadTemplate(string path)
        {
            using (var reader = new StreamReader(path))
            {
                this.htmlTemplate = reader.ReadToEnd();
            }

            return this.htmlTemplate;
        }

        /// <summary>
        /// Renders the item information into the template and returns the result [html] as string
        /// </summary>
        /// <param name="item">Object of type [Item]</param>
        internal string RenderItemInformation(string template)
        {
            string htmlResult = template;

            string keywordsJoinedByCommaAndSpace = string.Join(", ", this.KeyWords);
            string itemFeaturesWithListItemsTags = this.AppendListItemsAndCutTheirLenghtForFittingLayout(this.item.Features.Select(x => x.Content).ToList());

            string descriptionWithStrongTags = this.InsertStrongTags(this.item.DescriptionContent);
            string itemFeaturesWithStrongTags = this.InsertStrongTags(itemFeaturesWithListItemsTags);

            htmlResult = this.ExactReplace(htmlResult, KeyItemDescription, descriptionWithStrongTags);
            htmlResult = this.ExactReplace(htmlResult, KeyBullets, itemFeaturesWithStrongTags);
            htmlResult = this.ExactReplace(htmlResult, KeyKeywords, keywordsJoinedByCommaAndSpace);
            htmlResult = this.ExactReplace(htmlResult, KeyMaintitle, this.item.OriginalName);
            htmlResult = this.ExactReplace(htmlResult, KeyAltTitle, this.item.OriginalName);
            htmlResult = this.ExactReplace(htmlResult, KeyImageLink, this.itemsService.GetMainPictureLinkByItemId(this.item.Id));


            //var htmlMinifier = new HtmlMinifier();

            //var minifiedResult = htmlMinifier.Minify(htmlResult);
            //var minifiedHtml = minifiedResult.MinifiedContent;

            var minifiedHtml = Minifiers.Html.Compress(htmlResult);

            return minifiedHtml;
        }

        internal void SaveHtml(string path, string outputHtml)
        {
            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine(outputHtml);
            }
        }

        private List<string> GetKeywordsFromItemName(string itemName)
        {
            List<string> keywords = new List<string>();
            keywords = this.item.OriginalName.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            return keywords;
        }

        private List<string> GetRandomKeyWordsFromItemFeatures(List<string> itemFeatures, int wordsToGet)
        {
            string[] descriptionSeparators = new string[] { " ", ".", ",", "?", "!", "<strong>", "</strong>", "\r", "\n", "/", "\\", "<br>", "<br />", "-" };

            List<string> keywords = new List<string>();
            int counter = 0;
            var minimumKeywordLength = Constants.MINIMUM_KEYWORD_LENGTH;

            StringBuilder sb = new StringBuilder();

            var itemFeaturesList = this.item.Features
                                       .Select(x => x.Content);

            foreach (var itemFeature in itemFeaturesList)
            {
                sb.AppendLine(itemFeature);
            }

            var itemFeaturesToListOfKeywords = sb.ToString()
             .Split(descriptionSeparators, StringSplitOptions.RemoveEmptyEntries)
             .ToList();

            for (int i = 0; i < wordsToGet; i++)
            {
                Random rand = new Random(i + 4);
                int randNumber = rand.Next(0, itemFeaturesToListOfKeywords.Count - 1);
                var randomKeyWord = itemFeaturesToListOfKeywords[randNumber];
                if (randomKeyWord.Length > minimumKeywordLength)
                {
                    keywords.Add(randomKeyWord);
                }
                else
                {
                    wordsToGet++;
                }

                counter++;

                // In order not to get into infinite loop
                if (counter > 100)
                {
                    break;
                }
            }

            return keywords;
        }

        private List<string> GatherAllKeyWords(List<string> constantKeyWords, List<string> keyWordsFromName, List<string> randomKeyWords)
        {
            List<string> keywords = new List<string>();
            keywords.AddRange(constantKeyWords);
            keywords.AddRange(keyWordsFromName);
            keywords.AddRange(randomKeyWords);

            //keywords validations
            keywords = this.RemoveDuplicationsInList(keywords);
            keywords = this.TrimKeywords(keywords);
            keywords = this.RemoveIfInvalidKeyword(keywords);
            return keywords;
        }

        private List<string> RemoveIfInvalidKeyword(List<string> keywords)
        {
            List<string> validKeyWords = new List<string>();

            for (int i = 0; i < keywords.Count; i++)
            {
                var currentKeyword = keywords[i];

                bool isValid = Regex.IsMatch(currentKeyword, "^[a-zA-Z0-9]*$");

                if (isValid)
                {
                    validKeyWords.Add(currentKeyword);
                }
            }

            return validKeyWords;
        }

        private List<string> TrimKeywords(List<string> keywords)
        {
            List<string> trimmedKeywords = new List<string>();

            for (int i = 0; i < keywords.Count; i++)
            {
                var currentTrimmedKeyword = keywords[i].Trim();
                trimmedKeywords.Add(currentTrimmedKeyword);
            }

            return trimmedKeywords;
        }

        private List<string> RemoveDuplicationsInList(List<string> list)
        {
            List<string> unique = list.Distinct().ToList();
            return unique;
        }
        
        private string InsertStrongTags(string text)
        {
            foreach (var keyword in this.KeyWords)
            {
                int i = 0;
                int indexOfWord = text.IndexOf(keyword, i, StringComparison.CurrentCultureIgnoreCase);
                int strongTagLength = "<strong>".Length;
                int wordlength = keyword.Length + strongTagLength;

                while (indexOfWord != -1)
                {
                    text = text.Insert(indexOfWord, "<strong>");
                    text = text.Insert(indexOfWord + wordlength, "</strong>");
                    i = indexOfWord + wordlength + strongTagLength + 1;
                    indexOfWord = text.IndexOf(keyword, i, StringComparison.CurrentCultureIgnoreCase);
                }
            }

            return text;
        }

        private string AppendListItemsAndCutTheirLenghtForFittingLayout(List<string> itemFeatures)
        {
            StringBuilder bulletBuilder = new StringBuilder();

            foreach (var feature in itemFeatures)
            {
                bulletBuilder.Append("<li>");
                string currentBullet = feature.Trim();

                // ako bulleta ima nad 30 simvola mu davame \r\r 
                int bulletLength = currentBullet.Length;
                int maxLength = 37;
                if (bulletLength >= maxLength)
                {
                    // tuk go delim na maxlength - 7, za da ne ostavat dumi s po malko ot 7 simvola na edin red
                    int insertionTimes = int.Parse(Math.Ceiling((double)(bulletLength / maxLength - 7)).ToString());
                    int start = 30;
                    int firstIndexOfSpace = currentBullet.IndexOf(" ", start);
                    for (int i = 0; i < insertionTimes; i++)
                    {
                        if (firstIndexOfSpace == -1)
                        {
                            break;
                        }

                        if (firstIndexOfSpace + 15 < bulletLength && firstIndexOfSpace < bulletLength - 9)
                        {
                            currentBullet = currentBullet.Insert(firstIndexOfSpace, "<br />");
                            start += 32;
                            if (start >= bulletLength)
                            {
                                break;
                            }

                            firstIndexOfSpace = currentBullet.IndexOf(" ", start);
                        }
                    }

                    bulletBuilder.Append(currentBullet);
                }
                else
                {
                    bulletBuilder.Append(currentBullet);
                }

                bulletBuilder.Append("</li>");
            }

           string fixedLengthFeatureItemsList = bulletBuilder.ToString();
           return fixedLengthFeatureItemsList;
        }
        
        private string ExactReplace(string input, string wordToRemove, string wordToInsert)
        {
            string pattern = @"\b" + wordToRemove + @"\b";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            Match match = regex.Match(input);
            while (match.Success)
            {
                var currentWord = input.Substring(match.Index, match.Length);
                input = Regex.Replace(input, currentWord, wordToInsert);
                match = regex.Match(input);
            }

            return input;
        }
    }
}
