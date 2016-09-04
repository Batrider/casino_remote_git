/* ==============================================================================
 * 功能描述：错误码
 * 创 建 者：cjunhong
 * 创建日期：2015/02/03 20:43:09
 * 邮    箱：[url=mailto:john.cha@qq.com]john.cha@qq.com[/url]
 * ==============================================================================*/
using UnityEngine;
public class PointDetail
{
    
    /// <summary>
    /// 错误的id
    /// </summary>
    public int gameId{ get; set; }
    
    /// <summary>
    /// 错误的提示信息
    /// </summary>
    public Vector3 firstPoint{ get; set; }
    public Vector2 offsetPoint{ get; set; }

}
