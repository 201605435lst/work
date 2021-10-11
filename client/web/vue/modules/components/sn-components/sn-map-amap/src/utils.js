import Vue from 'vue';

//高德高德地图 方法封装
let vm = new Vue({});

let markerPostion=[];
//使用插件配置项
let plugin =
  'plugin=AMap.Autocomplete,AMap.PlaceSearch,AMap.Geocoder,AMap.Marker,AMap.DistrictSearch,AMap.Polygon';

export function amap(){
  let url =
    'https://webapi.amap.com/maps?v=1.4.15&key=429ffa93b08b7a39aa0e6f39605f96cc&callback=onload&' +
    plugin;
  let jsapi = document.createElement('script');
  jsapi.charset = 'utf-8';
  jsapi.src = url;
  document.head.appendChild(jsapi);
};
//添加样式
function getData(data,map,call) {
  let bounds =data.boundaries;
  let polygons = [];
  let polygon;
  if (bounds) {
    for (let i = 0, l = bounds.length; i < l; i++) {
      polygon = new AMap.Polygon({
        map:map,
        strokeWeight: 1,
        strokeColor: '#0091ea',
        fillColor: '#80d8ff',
        fillOpacity: 0.2,
        path: bounds[i],
      });
      polygons.push(polygon);
    }
    map.setFitView(); //地图自适应
    let marker = new AMap.Marker({});
    map.add(marker);
    polygon.on("click", function (e) {
      let lnglat = [e.lnglat.lng, e.lnglat.lat];
      marker.setPosition(lnglat);
      call(lnglat);
    });
  }
}
export const getLntlat = markerPostion;

export function getDistrict(adcode, map) {
  return new Promise((res, rej) => {
    let opts = {
      extensions: 'all',
      subdistrict: 1,   //返回下一级行政区
      showbiz: false,  //最后一级返回街道信息
    };
    let district = new AMap.DistrictSearch(opts);
    district.search(pad(adcode, 6), function (status, result) {
      if (status === 'complete') {
        getData(result.districtList[0], map, lnglat => {
          res(lnglat);
        });
      }
    });
  });
  
}

//逆地理编码 经纬度转具体地理位置
export function getAddress(lnglat) {
  let geocoder = new AMap.Geocoder();
  return new Promise((res, err) => {
    geocoder.getAddress(lnglat, function(status, result) {
      if (status === 'complete' && result.regeocode) {
        res(result.regeocode);
      } else {
        err("查询错误");
      }
    });
  });
}

//位数不够补齐0(后置)
export function pad(num, n) {
  let tbl = [];
  let len = n-num.toString().length;
  if (len <= 0) return num;
  if (!tbl[len]) tbl[len] = (new Array(len+1)).join('0');
  return num + tbl[len];
}