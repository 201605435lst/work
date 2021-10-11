
import './style';
import { requestIsSuccess } from '../../_utils/utils';
import ApiEntity from '../../sm-api/sm-namespace/Entity';
import SmSafeRltQualityModalSelectModal from './SmSafeRltQualityModalSelectModal';
let apiEntity = new ApiEntity();

export default {
  name: 'SmSafeRltQualityModalSelect',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
    };
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
    async refresh() { },
    add(){
      this.$refs.SmSafeRltQualityModalSelectModal.add();
    },
  },
  render() {
    return (
      <div class="sm-safe-rlt-quality-modal-select">
        <a-button type="primary" onClick={this.add}>点击选择</a-button>
        <SmSafeRltQualityModalSelectModal
          ref="SmSafeRltQualityModalSelectModal"
          onSuccess={(data)=>console.log(data)}
          axios={this.axios}
        />
      </div>
    );
  },
};
    