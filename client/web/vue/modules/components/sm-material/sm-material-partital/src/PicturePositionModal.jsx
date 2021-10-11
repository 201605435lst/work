/**
 * 说明：图片位置拾取
 * 作者：easten
 */

import PicturePosition from './PicturePosition';
export default{
  name: 'PicturePositionModal',
  components: {},
  props: {
    src: { type: String, default: '' },
  },
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
    picClick(point){
      this.point=point;
      console.log(point);
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
      title='分区选择'
      onOk={this.ok}
      visible={this.visible}
      onCancel={this.close}
      width={700}
      height={600}
      // bodyStyle={{ height: '300px' }}
      confirmLoading={this.confirmLoading}
    >
      <PicturePosition src={this.src} visible={true} view={false} mode='mark' onClick={this.picClick}/>
    </a-modal>;
  },
};