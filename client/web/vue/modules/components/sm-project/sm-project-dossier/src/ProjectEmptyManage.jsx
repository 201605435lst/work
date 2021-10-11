import "../style/index";

export default {
  name: 'ProjectEmptyManage',
  props: {
    axios: { type: Function, default: null },
    message: { type: String, default: "请选择你要管理的类" },//模态框类型
  },
  render() {
    return (
      <div class="sm-project-empty-manage">
        <a-card>
          <a-empty description={this.message} />
        </a-card>
      </div>
    );
  },
};
