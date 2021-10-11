import './style';
import { requestIsSuccess, stringfyScope, parseScope } from '../../_utils/utils';
import { ScopeType } from '../../_utils/enum';
import { ref, onMounted, watch, nextTick, computed } from '@vue/composition-api';
import useRailways from './src/useRailways';
import ApiStation from '../../sm-api/sm-basic/Station';
let apiStation = new ApiStation();

export default {
  name: 'SmD3StationSlider',
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false }, //面板是否弹出
    theme: { type: String }, // dark
    scopeCode: { type: String },
  },
  setup(props, ctx) {
    const { railways, activedRailWay, activeRailway } = useRailways(props.axios);
    const stations = ref([]);
    const activedStation = computed(() => stations.value.find(item => item.active));
    const center = ref(true);
    const swiperWrapperStationRef = ref(null);
    let swaperStations, swaperRailways;

    const initAxios = () => {
      apiStation = new ApiStation(props.axios);
    };
    const initRailwaySwaper = () => {
      nextTick(() => {
        swaperRailways = new Swiper('#railway-list', {
          slidesPerView: 'auto',
          mousewheel: true,
          on: {
            slideChange: function (data) {
              activeRailway(railways.value[this.activeIndex].id);
              nextTick(() => {
                initStations();
              });
            },
          },
          navigation: {
            nextEl: '.railway-swiper-button-next',
            prevEl: '.railway-swiper-button-prev',
          },
        });
      });
    };

    const prevRailway = () => {
      let index = railways.value.indexOf(activeRailway);
      swaperRailways.slideTo(index - 1);
    };
    const nextRailway = () => {
      let index = railways.value.indexOf(activeRailway);
      swaperRailways.slideTo(index + 1);
    };
    const initStations = async () => {
      stations.value = [];
      swaperStations && swaperStations.destroy();
      nextTick(async () => {
        if (!activedRailWay.value) return;
        let response = await apiStation.getListByRailwayId(activedRailWay.value.id);
        if (requestIsSuccess(response)) {
          stations.value = response.data
            .map(item => {
              item.enabled = true;
              item.active = false;
              return item;
            })
            .filter(item => item.type === 0)
            .sort((a, b) => a.kmMark - b.kmMark);
          // if (stations.value.length) {
          //   stations.value[0].active = true;
          // }
        }

        nextTick(() => {
          initStationSwaper();
        });
      });
    };
    const initStationSwaper = () => {
      swaperStations = new Swiper('#station-list', {
        slidesPerView: 'auto',
        mousewheel: true,
        navigation: {
          nextEl: '.station-swiper-button-next',
          prevEl: '.station-swiper-button-prev',
        },
      });

      nextTick(() => {
        let delta = swiperWrapperStationRef.value.offsetWidth;
        let children = swiperWrapperStationRef.value.children;

        for (let index = 0; index < children.length; index++) {
          let item = children[index];
          delta -= item.offsetWidth;
        }
        center.value = delta > 0;

        nextTick(() => {
          swaperStations.updateSize();
        });
      });
    };
    const activeStation = id => {
      stations.value.map(item => {
        item.active = item.id === id;
      });
    };
    const prevStation = () => {
      swaperStations.slidePrev();
    };
    const nextStation = () => {
      swaperStations.slideNext();
    };
    onMounted(() => {
      initAxios();
      initStations();
    });
    watch(railways, () => {
      initRailwaySwaper();
    });
    watch(activedRailWay, () => {
      initStations();
      flyToRaileway();

    });
    watch(activedStation, () => {
      let scope = activedStation.value
        ? stringfyScope(activedStation.value.id, ScopeType.Station, activedStation.value.name)
        : null;
      ctx.emit('change', scope);
    });
    watch(() => props.scopeCode, (value, oldValue) => {
      if (value) {
        let scope = parseScope(value);
        if (scope.type === ScopeType.Railway) {
          activeRailway(scope.id);
        } else if (scope.type === ScopeType.Station && scope.railwayScope) {
          activeRailway(scope.railwayScope.id);
          nextTick(() => {
            if (scope.id) {
              activeStation(scope.id);
              swaperStations && swaperStations.slideTo(stations.value.findIndex(x => x.id === scope.id), 1000, false);
            }
          });
        }
      } else {
        activeRailway(null);
        activeStation(null);
      }
    });
    const flyToRaileway = () => {
      activeStation(null);
      let scope = activedRailWay.value
        ? stringfyScope(activedRailWay.value.id, ScopeType.Railway, activedRailWay.value.name)
        : null;
      ctx.emit('change', scope);
    };

    return {
      railways,
      activedRailWay,
      activeRailway,
      nextRailway,
      prevRailway,
      stations,
      activedStation,
      activeStation,
      prevStation,
      nextStation,
      center,
      swiperWrapperStationRef,
      flyToRaileway,
    };
  },
  render() {
    return (
      <sc-panel
        class={{
          'sm-d3-station-slider': true,
          dark: this.theme === 'dark',
        }}
        height={'100px'}
        borderedRadius={true}
        showHeader={false}
        visible={this.visible}
        position={{
          bottom: '20px',
          left: '350px',
          right: '350px',
        }}
        animate="bottomEnter"
        bodyFlex
        forceRender
      >
        <div class="swiper-container railway-list" id="railway-list">
          <div class="swiper-wrapper">
            {this.railways.map((railway, index) => {
              return (
                <div
                  key={railway.id}
                  class={{
                    'swiper-slide': true,
                    'railway-item': true,
                    // active: this.activedRailWay && this.activedRailWay.id === railway.id,
                  }}
                  // onClick={this.flyToRaileway}
                >
                  <div class="title">{railway.name}</div>
                </div>
              );
            })}
          </div>
          <div class="railway-swiper-button-next swiper-button-next"></div>
          <div class="railway-swiper-button-prev swiper-button-prev"></div>
        </div>

        <div class="swiper-container station-list" id="station-list">
          <div
            class={{
              'swiper-wrapper': true,
              center: this.center,
            }}
            ref="swiperWrapperStationRef"
          >
            {this.stations.map((station, index) => {
              return (
                <div
                  key={station.id}
                  class={{
                    'swiper-slide': true,
                    'station-item': true,
                    enabled: station.enabled,
                    active: this.activedStation && this.activedStation.id === station.id,
                    first: index === 0,
                    last: index === this.stations.length - 1,
                  }}
                  onClick={() => this.activeStation(station.id)}
                >
                  <div class="line">
                    <div class="left"></div>
                    <div class="dot"></div>
                    <div class="right"></div>
                  </div>

                  <div class="title">{station.name}</div>
                </div>
              );
            })}
          </div>
        </div>
        {!this.center
          ? [
            <div class="station-swiper-button-next swiper-button-next"></div>,
            <div class="station-swiper-button-prev swiper-button-prev"></div>,
          ]
          : null}
      </sc-panel>
    );
  },
};
