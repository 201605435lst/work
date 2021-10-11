/**
 * 说明：高德地图封装
 * 作者：easten
 */

export default class amap{

  constructor(config){
    this.container=config.container;
    this.mapObj=null;
    this.setting={
      center:config.center|[],
      viewMode:config.viewMode|'3D',
      zoom:config.zoom|13,
    };
    this.markers=[];
    this.initMap();
  }
  initMap(){
    // 地图初始化
    this.mapObj=new AMap.Map(this.container,this.setting);
  }
  // 添加图层
  addLayer(layer){
    if(this.mapObj){
      this.mapObj.add(layer);
    }
  }
  // 添加标记
  createMarker(key,position,title,single){
    if(position[0]==undefined||position[1]==undefined){
      return;
    }
    let marker=new AMap.Marker({
      title,
    });
    let markerContent = document.createElement("div");
    let markerImg = document.createElement("img");
    markerImg.className = "markerlnglat";
    markerImg.src = "//a.amap.com/jsapi_demos/static/demo-center/icons/poi-marker-red.png";
    markerImg.setAttribute('width', '25px');
    markerImg.setAttribute('height', '34px');
    markerContent.appendChild(markerImg);
    marker.setContent(markerContent);
    marker.setPosition(position);
    if(single){
      this.removeMarker(key);
    }
    if(this.mapObj){
      this.mapObj.add(marker);
    }
    this.markers.push({
      key:key,
      marker:marker,
    });
    
    return this;
  }

  // 移出标记
  removeMarker(key){
    if(this.markers){
      let markers=this.markers.filter(a=>a.key==key);
      markers.map(item=>{
        if(this.mapObj){
          this.mapObj.remove(item.marker);
        }
      });
      this.markers=this.markers.filter(a=>a.key!=key);
    }
    return this;
  }
  // 获取一个点
  getPosition(point){
    return  new AMap.LngLat(point);    
  }
  // 添加监听
  addListener(event,call){
    if(this.mapObj){
      this.mapObj.on(event,(e)=>{
        call(
          [e.lnglat.getLng(),
            e.lnglat.getLat(),
          ]);
      });
    }
    return this;
  }
  // 移出监听
  removeListener(event){
    if(this.mapObj){
      this.mapObj.off(event,this.mapClick);
    }
  }
  // 设置等级
  setZoom(zoom){
    if(this.mapObj){
      this.mapObj.setZoom(zoom);
    }
    return this;
  }
  // 设置中心
  setCenter(point){
    if(this.mapObj){
      this.mapObj.setCenter(point);
    }
    return this;
  }
}