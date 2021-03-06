using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace HatCMS
{
    /// <summary>
    /// The CmsUserInterface class is used to integrate the HatCms.Core library with the user inteface.
    /// </summary>
    public class CmsUserInterface
    {
        public IFlashObjectBrowser FlashObjectBrowser;
        public IShowThumbnailPage ShowThumbnailPage;
        private CollectUIDependenciesDelegate _collectUIDependenciesDelegate = null;

        public delegate CmsDependency[] CollectUIDependenciesDelegate();


        public CmsUserInterface(IShowThumbnailPage showThumbnailPage, IFlashObjectBrowser flashObjectBrowser, CollectUIDependenciesDelegate collectUIDependenciesDelegate )
        {
            ShowThumbnailPage = showThumbnailPage;
            FlashObjectBrowser = flashObjectBrowser;
            _collectUIDependenciesDelegate = collectUIDependenciesDelegate;
        }

        public CmsDependency[] CollectUIDependencies()
        {
            if (_collectUIDependenciesDelegate != null)
                return _collectUIDependenciesDelegate();
            
            return new CmsDependency[0];
        }

    }

    public interface IShowThumbnailPage
    {
        string ThumbImageCacheDirectory { get; }
        string getThumbDisplayUrl(string imgPath, System.Drawing.Size displayBoxSize);
        string getThumbDisplayUrl(string imgPath, int displayBoxWidth, int displayBoxHeight);
        string getThumbDisplayUrl(CmsLocalImageOnDisk resource, int displayBoxWidth, int displayBoxHeight);
        System.Drawing.Size getDisplayWidthAndHeight(string fileUrl, System.Drawing.Size displayBox);
        System.Drawing.Size getDisplayWidthAndHeight(string fileUrl, int displayBoxWidth, int displayBoxHeight);
        System.Drawing.Size getDisplayWidthAndHeight(CmsLocalImageOnDisk resource, int displayBoxWidth, int displayBoxHeight);
    }

    public interface IFlashObjectBrowser
    {
        int PopupHeight { get; }
        int PopupWidth { get; }
        bool DirHasSWFFiles(DirectoryInfo di);
        string getUrl(string JSCallbackFunctionName);
        FileInfo[] GetFlashFiles(DirectoryInfo di);
    }
}
