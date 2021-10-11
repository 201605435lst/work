
import './style';
import { requestIsSuccess } from '../../_utils/utils';
import ApiProject from '../../sm-api/sm-project/Project';
import ApiSubItem from '../../sm-api/sm-construction-base/ApiSubItem';
import ApiProcessTemplate from '../../sm-api/sm-std-basic/ProcessTemplate';
import ApiSubItemContent from '../../sm-api/sm-construction-base/ApiSubItemContent';
import SmConstructionBaseSubItemSelectTreeModal from './SmConstructionBaseSubItemSelectTreeModal';
let apiProject = new ApiProject();
let apiSubItem = new ApiSubItem();
let apiProcessTemplate = new ApiProcessTemplate();
let apiSubItemContent = new ApiSubItemContent();

export default {
  name: 'SmConstructionBaseSubItemSelectTree', // 分部分项选择树
  props: {
    axios: { type: Function, default: null }, // axios
    bordered: { type: Boolean, default: true }, // 边框模式
    disabled: { type: Boolean, default: false }, // 编辑模式和查看模式
    value: { type: [Array, String] }, // 已选择的内容 (单选数组,多选string )
    multiple: { type: Boolean, default: false }, //是否多选，默认单选
    placeholder: { type: String, default: '请点击选择分部分项' },
    height: { type: Number, default: 100 }, // 当前选择框的大小
  },
  data() {
    return {
      iValue:[],
      iVisible: false,
      selectedSubItems: [], //已选择区段
    };
  },
  computed: {
    visible() {
      return this.iVisible;
    },
    tags() {
      return this.selectedSubItems;
    },
  },
  watch: {
    value: {
      handler(nVal, oVal) {
        if (this.multiple) {
          this.iValue = nVal;
        } else {
          this.iValue = [nVal];
        }
        this.initSubItem();
      },
      // immediate: true,
    },
  },
  async created() {
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiProject = new ApiProject(this.axios);
      apiSubItem = new ApiSubItem(this.axios);
      apiSubItemContent = new ApiSubItemContent(this.axios);
      apiProcessTemplate = new ApiProcessTemplate(this.axios);
    },

    selected(value) {
      this.selectedSubItems = value;
      if (this.multiple) {
        this.$emit(
          'change',
          this.selectedSubItems && this.selectedSubItems.length > 0
            ? this.selectedSubItems.map(item => item.id)
            : [],
        );
      } else {
        this.$emit(
          'change',
          this.selectedSubItems[0] ? this.selectedSubItems[0].id : null,
        );
      }
    },

    //已选工序规范数据初始化
    initSubItem() {
      let _selectedSubItem = [];
      if (this.iValue.length > 0) {
        this.iValue.map(async id => {
          if (id) {
            let response = await apiSubItemContent.get(id);
            if (requestIsSuccess(response)) {
              _selectedSubItem.push(response.data);
            }
          }
        });
      }
      this.selectedSubItems = _selectedSubItem;
    },

    // 鼠标点击 大框
    faultEquipmentSelect() {
      this.iVisible = true;
    },

  },
  render() {
    let subItems = this.tags.map(item => {
      return <div class="selected-item">
        <div class="selected-name"> {item ? item.name : null} </div>
        {!this.disabled ?
          <span
            class="btn-close"
            onClick={e => {
              e.stopPropagation();
              this.iValue = this.iValue.filter(id => id !== item.id);
              this.selectedSubItems = this.selectedSubItems.filter(_item => _item.id !== item.id);
              if (this.multiple) {
                this.$emit(
                  'change',
                  this.iValue && this.iValue.length > 0
                    ? this.iValue.map(item => item)
                    : [],
                );
              } else {
                this.$emit(
                  'change',
                  this.iValue[0] ? this.iValue[0].id : null,
                );
              }
            }}
          >
            <a-icon type="close" />
          </span> : undefined}
      </div>;
    });
    return (
      <div
        class={{
          'subItem-select-panel': true,
          'ant-input': true,
          bordered: this.bordered,
        }}
        style={{
          height: this.bordered ? this.height + 'px' : 'auto',
        }}
        onClick={() => this.faultEquipmentSelect()}>
        {this.selectedSubItems.length === 0 ? <label class="tip">{this.placeholder}</label> : <div class="selected">{subItems}</div>}
        {/*分布分项树选择框*/}
        <SmConstructionBaseSubItemSelectTreeModal
          axios={this.axios}
          visible={this.iVisible}
          value={this.selectedSubItems}
          multiple={this.multiple}
          onOk={this.selected}
          onChange={visible => (this.iVisible = visible)}
        />
      </div>
    );
  },
};
