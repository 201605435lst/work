import './style';
import ApiSection from '../../sm-api/sm-construction-base/ApiSection';

let apiSection = new ApiSection();

// 选择公用的div
export default {
  name: 'SelectCommonDiv',
  props: {
    bordered: { type: Boolean, default: true }, // 边框模式
    tags: { type: Array, default: ()=>[] }, // tag列表
    placeholder: { type: String, default: '请选择' },
    disabled: { type: Boolean, default: false }, // 编辑模式和查看模式
    height: { type: Number, default: 100 }, // 当前选择框的大小
  },
  data() {
    return {};
  },
  computed: {},
  watch: {},
  async created() {

  },
  methods: {},
  render() {
    return (
      <div class='select-common-div'>
        <div class={{ 'select-panel': true, 'ant-input': true, bordered: this.bordered }}
          style={{ height: this.bordered ? this.height + 'px' : 'auto' }}
          onClick={() => {
            this.$emit('click');
          }}>
          {this.tags.length === 0
            ? <label class='tip'>{this.placeholder}</label>
            : <div class='selected'>{
              this.tags.map(item => <div class='selected-item'>
                <div class='selected-name'> {item ? item.name : null} </div>
                {!this.disabled &&
                <span
                  class='btn-close'
                  onClick={e => {
                    e.stopPropagation(); //取消外面包的div 的click
                    this.$emit('delClick',item);
                  }}
                >
                  <a-icon type='close' />
                </span>}
              </div>)
            }</div>
          }
        </div>
      </div>
    );
  },
};
