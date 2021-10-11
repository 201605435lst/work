import { ModalStatus } from '../../_utils/enum';
export default{
  name: 'SmBpmWorkflowTemplatesGroupModal',
  components: {},
  props: {},
  data() {
    return {
      status:ModalStatus.Hide,
      groupName:'',
    };
  },
  computed: {
    visible() {
      return this.status != ModalStatus.Hide;
    },
  },
  watch: {},
  created() {},
  methods: {
    add(){
      this.status = ModalStatus.Add;
      this.groupName="";
    },
    edit(name){
      this.groupName=name;
      this.status = ModalStatus.Edit;
    },
    ok(){
      this.$emit("success",this.groupName);
      this.close();
    },
    close(){
      this.status = ModalStatus.Hide;
    },
  },
  render() {
    return (
      <a-modal
        title={this.edit?'分组编辑':'分组新增'}
        onOk={this.ok}
        visible={this.visible}
        onCancel={this.close}
        bodyStyle={{ height: '150px' }}
      >
        <a-form>
          <a-form-item
            label="分组名称"
          >
            <a-input value={this.groupName} onChange={e=>{this.groupName = e.target.value.replace(' ','');}}></a-input>
          </a-form-item>
        </a-form>
      </a-modal>
    );;
  },
};