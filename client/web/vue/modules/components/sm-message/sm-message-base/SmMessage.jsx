
let bpmSignalR = null;
export default {
  name: 'SmMessage',
  components: {},
  props: {
    axios: { type: Function, default: null },
    signalr: { type: Function, default: null },
  },
  data() {
    return {
      bmpTopic: "bpm",
      receiveMethod: "ReceiveMessage",
      processMethod: "Process",
      receiveContent: "",
    };
  },
  computed: {
    content() {
      return this.receiveContent;
    },
  },
  watch: {},
  created() {

    //this.initSignalr();
  },
  methods: {
    // initSignalr() {
    //   bpmSignalR = new this.signalr(this.bmpTopic);
    //   bpmSignalR.init((data) => {
    //     debugger;
    //   }).on(this.receiveMethod, (type, res) => {
    //     debugger;
    //   });
    //},
    test() {
      bpmSignalR.send(this.processMethod, "");
    },
  },
  render() {
    return (<div>
      <a-button onClick={() => this.test()}>测试</a-button>
      <a-textarea placeholder="等待接收服务端消息" rows={4} value={this.content} />
    </div>);
  },
};
