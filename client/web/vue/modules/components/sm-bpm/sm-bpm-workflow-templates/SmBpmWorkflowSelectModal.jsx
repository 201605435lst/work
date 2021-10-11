import SmBpmWorkflowTemplates from './SmBpmWorkflowTemplates';
export default{
  name: 'SmBpmWorkflowSelectModal',
  components: {SmBpmWorkflowTemplates},
  props: {
    axios:{type:Function,default:()=>null},
    width:{type:Number,default:800},
    height:{type:Number,default:450},
  },
  data() {
    return {
      visible:false,
      selectedId:'',
    };
  },
  computed: {},
  watch: {},
  created() {},
  methods: {
    show(){
      this.visible=true;
      this.selectedId='';
    },
    close(){
      this.visible=false;
      this.$refs.flowTemplate.reset();
    },
    ok(){
      if(this.selectedId==null||this.selectedId==""){
        this.$message.warn("请选择审批流程");
      }
      this.$emit('selected',this.selectedId);
      this.close();
    },
  },
  render() {
    return  <a-modal
      title='流程选择'
      onOk={this.ok}
      visible={this.visible}
      onCancel={this.close}
      width={this.width}
      height={this.height}
      // bodyStyle={{ height: '600px' }}
    >
      <SmBpmWorkflowTemplates ref="flowTemplate" axios={this.axios} select={true} onSuccess={(key)=>this.selectedId=key} forCurrentUser/>
    </a-modal>;
  },
};
