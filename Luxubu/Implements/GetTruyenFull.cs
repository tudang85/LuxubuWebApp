using Luxubu.Iterfaces;
using Luxubu.Models;
using Luxubu.Types;
using NSoup;
using NSoup.Nodes;
using NSoup.Select;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luxubu.Implements
{
    public class GetTruyenFull : IGetStory
    {
        private string strStoryUrl = string.Empty;
        public GetTruyenFull(string strUrl)
        {
            strStoryUrl = strUrl;
        }

        public bool Download()
        {
            throw new NotImplementedException();
        }

        public Chapters[] GetChapters(long lngStoryId)
        {
            try
            {
                Document objDocChapter = null;
                int intPage = 1;
                List<Chapters> objChapterList = new List<Chapters>();
                bool blnHasClass = false;
                do
                {
                    objDocChapter = NSoupClient.Connect($"{strStoryUrl}/trang-{intPage}/#list-chapter")
                        .UserAgent(ConstValue.USER_AGENT_CHROME)
                        .Get();
                    Elements arrChapters = objDocChapter.Select(".list-chapter");
                    foreach (var objList in arrChapters)
                    {
                        foreach (var item in objList.Children)
                        {
                            Element objChapterElement = item.Select("a").First;
                            objChapterList.Add(new Chapters()
                            {
                                StoryId = lngStoryId,
                                Status = 0,
                                Url = objChapterElement.Attr("href"),
                                Name = objChapterElement.Attr("title"),
                            });
                        }
                    }
                    // Kiem tra xem da den trang cuoi cung chua
                    Element objEleLi = objDocChapter.Select(".dropup.page-nav").First;
                    Element objEleLiPrevious = objEleLi.PreviousElementSibling;
                    blnHasClass = objEleLiPrevious.HasClass("active");
                    intPage++;
                } while (!blnHasClass);
                return objChapterList.ToArray();
            }
            catch (Exception objEx)
            {
                throw objEx;
            }
        }

        public Stories GetInfo()
        {
            try
            {
                Document objDoc = NSoupClient.Connect(strStoryUrl)
                    .UserAgent(ConstValue.USER_AGENT_CHROME)
                    .Get();
                OutputSettings settings = new OutputSettings();
                settings.SetEncoding(Encoding.UTF8);
                objDoc.OutputSettings(settings);
                string strTitle = objDoc.Title;
                Element objEleAuthor = objDoc.Select(".info > div > a[itemprop=author]").First;
                string strAuthor = objEleAuthor.Text();
                Element objEleDesc = objDoc.Select("div[itemprop=description]").First;
                string strDesc = System.Web.HttpUtility.HtmlDecode(objEleDesc.Html());
                Element objEleCover = objDoc.Select("img[itemprop=image]").First;
                string strCover = objEleCover.Attr("src");
                Stories objStory = new Stories()
                {
                    Author = strAuthor,
                    Cover = strCover,
                    Desc = strDesc,
                    Title = strTitle
                };
                return objStory;
            }
            catch (Exception objEx)
            {
                throw objEx;
            }
        }
    }
}
