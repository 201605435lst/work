

export const InitMapScript=async function(call){
  
  return new Promise((a,b)=>{
    let script=document.createElement('script');
    script.id='amap';
    script.type='text/javascript';
    script.charset = 'utf-8';
    script.src='https://webapi.amap.com/maps?v=2.0&key=429ffa93b08b7a39aa0e6f39605f96cc&callback=mapload&';
    // 判断是否存在脚本
    let ish=document.getElementById('amap');
    if(ish!=null){
      document.head.removeChild(ish);
    }
    document.head.appendChild(script);
    window.mapload=function(){
      a();
      
    };
  });  
};