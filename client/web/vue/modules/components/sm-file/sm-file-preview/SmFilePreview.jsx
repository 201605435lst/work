
import './style';
import { requestIsSuccess, getFileUrl } from '../../_utils/utils';
// import mammoth from 'mammoth';
export default{
  name: 'SmFilePreview',
  components: {},
  props: {},
  data() {
    return {
      worldHtml:'',
    };
  },
  computed: {},
  watch: {},
  created() {

  },
  methods: {
    convertToHtml(path){
      mammoth.convertToHtml({path:'http://172.16.1.22:8072/sn-public/2020/12/39f9ba92-0eec-ce9b-6cf3-c5285c993cd2.docx'}).then((res)=>{
        debugger;
      }).done();
    },
    open(){
      let path="/2020/12/39f9ba92-0eec-ce9b-6cf3-c5285c993cd2.docx";
      // this.convertToHtml(getFileUrl(path));
    },
  },
  render() {
    return <div>
      <a-button onClick={()=>this.open()}>点击</a-button>
    </div>;
  },
};
