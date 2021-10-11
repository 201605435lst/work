/**
 * 说明：地图位置拾取
 * 作者：easten
 */

import SmMapControl from '../../../sm-common/sm-map-control';
export default{
  name: 'PositionModal',
  components: {},
  props: {},
  data() {
    return {
      visible:false,
      point:[],
    };
  },
  computed: {},
  watch: {},
  created() {},
  methods: {
    mapClick(point){
      this.point=point;
    },
    ok(){
      this.$emit('success',this.point);
      this.close();
    },
    view(){
      point:[];
      this.visible=true;
    },
    close(){
      this.visible=false;
    },
  },
  render() {
    return  <a-modal
      title='分区分区选择'
      onOk={this.ok}
      visible={this.visible}
      onCancel={this.close}
      width={700}
      height={600}
      // bodyStyle={{ height: '300px' }}
      confirmLoading={this.confirmLoading}
    >
      <SmMapControl singlePoint={true} visible={true} mode='mark' onClick={this.mapClick}/>
    </a-modal>;
  },
};