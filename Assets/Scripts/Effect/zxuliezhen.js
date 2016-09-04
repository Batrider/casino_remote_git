var frames : Texture[];  

var framesPerSecond = 10;  //声明fps,每秒播放几帧
function Update() {
   var index : int = (Time.time * framesPerSecond) % frames.Length;    //数组的索引，根据时间改变，当前时间乘以fps与总帧数取余，就是播放的当前帧，随着update更新
   renderer.material.mainTexture = frames[index];    
}

