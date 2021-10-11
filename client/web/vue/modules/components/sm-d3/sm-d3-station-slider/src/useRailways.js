import { ref, onMounted, onUnmounted, computed } from '@vue/composition-api';
import ApiRailway from '../../../sm-api/sm-basic/Railway';
import { requestIsSuccess } from '../../../_utils/utils';

export default function useRailways(axios) {
  const railways = ref([]);
  let apiRailway;

  onMounted(async () => {
    apiRailway = new ApiRailway(axios);
    let response = await apiRailway.getList();
    if (requestIsSuccess(response)) {
      response.data.items.map(item => {
        item.acitved = false;
      });
      let _railways = JSON.parse(JSON.stringify(response.data.items));
      if (_railways.length) {
        // _railways[_railways.length - 1].acitved = true;
        _railways[0].acitved = true;
      }
      railways.value = _railways;
    }
  });

  const activeRailway = id => {
    railways.value.map(item => {
      item.acitved = item.id === id;
    });
  };

  const activedRailWay = computed(() => {
    return railways.value.find(x => x.acitved);
  });

  return { railways, activedRailWay, activeRailway };
}
