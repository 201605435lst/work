import { amap,getAddress,pad} from "./src/utils";
export default {
  name: "SnMapAmap",
  props:{
    height: { type: Number, default: 300 },
    width: { type: Number, default: 600 },
    isD3: { type: Boolean, default: true },//是否为3D模式
    disabled: { type: Boolean, deafault: false },
    address: { type: String, deafault: null },
    adcode: { type: String, default: null },
    centerValue:{type:Array,default:null},
  },
  data() {
    return {
      map: null, //高德地图
      visible: false,
      _adcode: null,
      center: [],
      polygons: [],
      result: {
        lat: "",//纬度
        lng: "",//经度
        address: "",//地址
        region: {},//区划信息
        name:"",//名称
      },
    };
  },
  computed: {
    // getLevel() {
    //   let level = null;
    //   switch (this._adcode.length) {
    //   case 2:
    //     level = "province";
    //     break;
    //   case 4:
    //     level = "city";
    //     break;
    //   case 6:
    //     level = "district";
    //     break;
    //   default:
    //     level = "";
    //     break;
    //   }
    //   return level;
    // },
  },
  watch: {
    address: {
      handler: function (value, oldValue) {
        if (value) {
          this.result.address = value;
        }
      },
    },
    adcode: {
      handler: function (value, oldValue) {
        if (value) {
          this._adcode = value;
          let level=null;
          switch (this._adcode.length) {
          case 2:
            level = "province";
            break;
          case 4:
            level = "city";
            break;
          case 6:
            level = "district";
            break;
          default:
            level = "";
            break;
          }
          this.search(this._adcode, this.map, this,level);
          // }
        }
      },
    },
    centerValue: {
      handler: function (value, oldValue) {
        if (value) {
          this.center = value;
        }
      },
    },
  },
  created() { this.init();},
  methods: {
    init() {
      let vm = this;
      amap();
      window.onload = function () {
      //创建高德地图
        vm.map = new AMap.Map("container", {
          zoom: 8, 
          resizeEnable: true, 
          viewMode: this.D3 ? '3D' : '',
          result: {},
        });
        let map = vm.map;
        vm.center.length > 0 ? map.setCenter(vm.center) : "";
        //搜索提示
        let autoOptions = {
          city: '全国',
          input: "search",
        };
        let auto = new AMap.Autocomplete(autoOptions);
        let placeSearch = new AMap.PlaceSearch({
          map: map,
        });
        //构造地点查询类
        AMap.event.addListener(auto, "select", select);//注册监听，当选中某条记录时会触发
        function select(e) {
          vm.result = {
            address: e.poi.address,
            name: e.poi.name,
            lat: e.poi.location.lat,
            lng: e.poi.location.lng,
            region: e.poi.district,
          };
          vm.result.address = e.poi.name;
          // placeSearch.setCity(e.poi.adcode);
          placeSearch.search(vm.result.address, function (status, result) { //关键字查询查询
            if (status == "complete" && result) {
              vm.$emit("change", vm.result);
            }
          }); 
        };
        vm.search(vm._adcode,map,vm,"province");
      };
    },
    search(adcode,map,vm,level) {
      //绘制行政区域
      let opts = {
        extensions: 'all',
        subdistrict: 1,   //返回下一级行政区
        showbiz: false,  //最后一级返回街道信息
      };
      
      // 清除地图上所有覆盖物;
      for (let i = 0, l = vm.polygons.length; i < l; i++) {
        vm.polygons[i].setMap(null);
      }
      let district = new AMap.DistrictSearch(opts);
      district.setLevel(level);
      console.log(level);
      district.search(pad(adcode, 6), function (status, result) {
        if (status === 'complete') {
          let bounds = result.districtList[0].boundaries;
          let polygon;
          if (bounds) {
            for (let i = 0, l = bounds.length; i < l; i++) {
              polygon = new AMap.Polygon({
                map: map,
                strokeWeight: 1,
                strokeColor: '#0091ea',
                fillColor: '#80d8ff',
                fillOpacity: 0.2,
                path: bounds[i],
              });
              vm.polygons.push(polygon);
            }
            map.setFitView(); //地图自适应
            let marker = new AMap.Marker({});
            map.add(marker);
            polygon.on("click", function (e) {
              let lnglat = [e.lnglat.lng, e.lnglat.lat];
              marker.setPosition(lnglat);
              getAddress(lnglat).then(res => {
                vm.result = {
                  lat: e.lnglat.lat,//纬度
                  lng: e.lnglat.lng,//经度
                  address: res.formattedAddress,
                  name: res.formattedAddress,
                  region: res.addressComponent,
                };
                vm.$emit("change", vm.result);
              }).catch(err => {
              });
            });
          }
        }
      });
    },
  },
  render() {
    return (
      [
        <div class="SnMapAmap" >
          <a-input id="search" class="search"
            placeholder="请输入关键字"
            onFocus={() => {
              this.visible =true;
            }}
            disabled = {this.disabled}
            value={this.result.address}
          ></a-input>
          <div style={`width:${this.width}px;height:${this.height}px;display:${this.visible ? 'block' : 'none'}`}>
            <div id="container" style={`width:${this.width}px;height:${this.height}px`}></div>
            <div style="float: right;marginTop: 6px">
              <a-button style="marginRight: 10px;"
                type="primary"
                size="small"
                onClick={() => {
                  this.$emit('ok', this.result);
                  this.visible = false;
                }}>确定</a-button>
              <a-button type="danger" size="small" onClick={() => { this.visible = false; this.result = {}; }}>取消</a-button>
            </div>
          </div>
        </div>,
      ]
    );
  },
};