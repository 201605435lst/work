import { requestIsSuccess } from '../../_utils/utils';
import ApiProject from '../../sm-api/sm-project/Project';
let apiProject = new ApiProject();

export default {
  name: 'SmProjectProjectSelect',
  model: {
    prop: 'value',
    event: ['input', 'change'],
  },
  props: {
    axios: { type: Function, default: null },
    value: { type: String || Number, default: undefined },
    disabled: { type: Boolean, default: false },
    placeholder: { type: String, default: '请选择' },
    size: { type: String, default: 'default' },
  },
  data() {
    return {
      projects: [], // 列表数据源
      iValue: undefined,
    };
  },
  computed: {},
  watch: {
    value: {
      handler: function (val, oldVal) {
        if (val) {
          this.initAxios();
          this.refresh();
        }
        this.iValue = val;
      },
      immediate: true,
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiProject = new ApiProject(this.axios);
    },
    async refresh() {
      let response = await apiProject.getList({ isAll: true });
      if (requestIsSuccess(response) && response.data.items) {
        let _projects = response.data.items.map(item => {
          return {
            ...item,
            title: item.name,
            value: item.id,
            key: item.id,
          };
        });

        this.projects = _projects;
        if (this.projects.length === 0) {
          this.iValue = undefined;
          this.$emit('input', this.iValue);
          this.$emit('change', this.iValue);
        }
      }
    },
  },
  render() {
    return (
      <a-select
        disabled={this.disabled}
        allowClear
        size={this.size}
        options={this.projects}
        placeholder={this.disabled ? '' : this.placeholder}
        style="width: 100%"
        value={this.iValue}
        onChange={value => {
          this.iValue = value;
          this.$emit('input', value);
          this.$emit('change', value);
        }}
      />
    );
  },
};
