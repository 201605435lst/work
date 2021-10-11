import './style/index.less';
import SmFlowBase from '../../sm-common/sm-flow-base';
export default {
  name: 'ScTest',
  props: {
    axios:{type:Function,default:null},
  },
  data() {
    return {
      nodes:[],
      filedTrans:{
        title:'name',
      },
      list:[],
    };
  },
  computed:{
    iNodes(){
      return this.nodes;
    },

  },
  created() {},
  methods:{
    async request(){
      let res=await this.axios.get("http://localhost:8091/api/app/planTest/getSingleFlowNodes?id=39fa35eb-8859-16fb-9a83-6a09fe4f7e8b");
      this.nodes=res.data;
      console.log(res.data);
    },
    async approve(id){
      let res=await this.axios({
        url: `http://localhost:8091/api/app/planTest/approveTest`,
        method: 'post',
        params:{id},
      });
      console.log(res.data);
    },
    async stoped(){
      let res=await this.axios({
        url: `http://localhost:8091/api/app/planTest/stoppedTest`,
        method: 'post',
        params:{id:"39fa2c1b-c901-1a44-0ba5-9003f061bb96"},
      });
      console.log(res.data);
    },
    async rejected(){
      let res=await this.axios({
        url: `http://localhost:8091/api/app/planTest/rejectedTest`,
        method: 'post',
        params:{id:"39fa2c1b-c901-1a44-0ba5-9003f061bb96"},
      });
      console.log(res.data);
    },
    async getList(){
      let res=await this.axios({
        url: `http://localhost:8091/api/app/planTest/getList`,
        method: 'get',
      });
      console.log(res.data);
      this.list=res.data.items;
    },
    async create(){
      let res=await this.axios({
        url:'http://localhost:8091/api/app/planTest/createPlan',
        method:'post',
        data:{
          name: "666",
          workFlowTemplateId: "709a6d69-d20e-43fc-97b5-6c95cd6d9c77",
        },
      });
      console.log(res.data);
    },
  },
  render() {
    return <div class="">
      <a-button onClick={this.create}>创建记录</a-button>
      <a-button onClick={this.request}>查询</a-button>
      <a-button onClick={this.getList}>查询列表</a-button>
      <SmFlowBase nodes={this.iNodes} filedTrans={this.filedTrans}/>
      <ul>
        {this.list.map(a=>{
          return <li>{a.name}{a.isWaiting?<a-button onClick={()=>this.approve(a.id)}>通过</a-button>:null}
          </li>;
        })}

      </ul>
      {/* // <a-button onClick={this.approve}>通过</a-button>
      // <a-button onClick={this.stoped}>退回</a-button>
      // <a-button onClick={this.rejected}>终止</a-button> */}

    </div>;
  },
};
