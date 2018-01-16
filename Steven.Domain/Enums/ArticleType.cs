using System.ComponentModel;

namespace Steven.Domain.Enums
{
    public enum ArticleListType
    {
        [Description("标题+日期")]
        Text = 1,
        [Description("标题+图片")]
        Image,
        [Description("视频")]
        Video
    }

    public enum ArticleDetailType
    {
        [Description("图文模板")]
        Article = 1,
        [Description("视频模板")]
        Video = 2,
        [Description("高清模板")]
        Image = 3
    }

    public enum ArticleFlags
    {
        [Description("无")]
        Nothing = 0,
        [Description("推荐")]
        Recommend = 1,
        [Description("置顶")]
        Top = 2
    }

    public enum ArticleTarget
    {
        [Description("本页")]
        ThisPage = 0,
        [Description("来源页")]
        SourcePage
    }
}
