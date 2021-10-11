import { requestIsSuccess } from '../../_utils/utils';
import ApiEntity from '../../sm-api/sm-namespace/Entity';
let apiEntity = new ApiEntity();

import { ref, reactive, watch, shallowReactive, onMounted } from '@vue/composition-api';
let AlarmSignalr = null;

import videojs from 'video.js';
import 'video.js/dist/video-js.css';

export default {
  name: 'SmNamespaceModule',
  props: {
    axios: { type: Function, default: null },
    signalr: { type: Function, default: null },
  },
  data() {
    return {};
  },
  setup(props, ctx) {
    console.log('--------------------------setup');
    let count = ref(10);
    count.value = 300;
    console.log(count.value);

    let add = () => {
      count.value++;
      console.log(count.value);
    };

    watch(count, () => {
      console.log('count is changed');
      AlarmSignalr.send('Register', count);
    });

    AlarmSignalr = new props.signalr('alarm');
    AlarmSignalr.init(() => {});
    // .on("equipment", data => {
    //   console.log('equipment', data);
    // });

    AlarmSignalr.on('Alarms', data => {
      console.log('Alarms', data);
    });

    /** video */
    let $video = ref(null);
    let player;
    onMounted(() => {
      console.log($video);
      player = videojs(
        'video-js',
        {
          controls: true,
          sources: [
            {
              src:
                'http://172.16.1.220:9000/sn-public/2020/12/39f9ab51-d149-ea00-f325-38590ae082ca.mp4',
              type: 'video/mp4',
            },
          ],
        },
        function onPlayerReady() {
          console.log('onPlayerReady');
        },
      );
    });

    return { count, $video, add };
  },
  computed: {},
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiEntity = new ApiEntity(this.axios);
    },
    async refresh() {},
  },
  render() {
    return (
      <div class="SmNamespaceModule">
        SmNamespaceModule --------------------
        <br />
        {this.count}
        <a-button onClick={this.add}>Add</a-button>
        <br />
        <br />
        <video ref={this.$video} id="video-js" class="video-js"></video>
      </div>
    );
  },
};
