import { requestIsSuccess, initBackgroundTask } from '../../../_utils/utils';
import ApiEquipments from '../../../sm-api/sm-resource/Equipments';

let apiEquipments = new ApiEquipments();

export default {
  name: 'SmResourceEquipmentsGenerateQuality',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      taskKey: "GenerateQuantity",
      progress: "",
      message: "",
      count: 0,
      index: 0,
    };
  },
  computed: {

  },
  created() {
    this.initAxios();
    this.refreshTask(this.taskKey);

  },
  methods: {
    initAxios() {
      apiEquipments = new ApiEquipments(this.axios);
    },

    async generate() {
      let taskKey = "GenerateQuantity";
      apiEquipments.generateQuantity(taskKey);
      this.refreshTask(taskKey);
    },

    refreshTask(taskKey) {
      initBackgroundTask(
        taskKey,
        this.axios,
        {
          onProgress: (data) => {
            this.progress = data.progress;
            this.message = data.message;
            this.count = data.count;
            this.index = data.index;
          },
          onSuccess: () => {
            console.log("已完成");
            this.progress = null;
            this.message = null;
            this.count = null;
            this.index = null;
          },
          onFailure: (data) => {
            console.log("失败");
          },
        },
      );
    },

  },
  render() {
    return <a-button loading={!!this.progress} onClick={this.generate} >生成工程量
      {this.progress ? <span>{`（${parseInt(this.progress * 100)}% ${this.index}/${this.count} ${this.message}）`}</span> : null}
    </a-button>;
  },
};
