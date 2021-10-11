// 文件选择对话框
import SmOaSealsSelectModal from './SmOaSealsSelectModal';
import { requestIsSuccess } from '../../_utils/utils';
import ApiSeal from '../../sm-api/sm-oa/Seal';

import './style/index.less';

let apiSeal = new ApiSeal();

export default {
  name: 'SmOaSealsSelect',

  model: {
    prop: 'value',
    event: 'change',
  },

  props: {
    axios: { type: Function, default: null },
    height: { type: Number, default: 80 }, // 当前选择框的大小
    disabled: { type: Boolean, default: false }, // 编辑模式和查看模式
    value: { type: [Array, String] }, // 已选择的内容
    placeholder: { type: String, default: '请点击选择签章' },
    bordered: { type: Boolean, default: true }, // 边框模式
  },

  data() {
    return {
      iValue: [],
      iVisible: false,
      selectedSeals: [],//已选择签章
    };
  },

  computed: {
    visible() {
      return this.iVisible;
    },
    tags() {
      return this.selectedSeals;
    },
  },

  watch: {
    value: {
      handler(nVal, oVal) {
        this.iValue = [nVal];
        this.initSeals();
      },
      // immediate: true,
    },
  },

  created() {
    this.initAxios();
    this.initSeals();
  },

  methods: {
    initAxios() {
      apiSeal = new ApiSeal(this.axios);
    },

    sealSelect() {
      if (!this.disabled) {
        this.iVisible = true;
      }
    },

    //已选签章数据初始化
    initSeals() {
      let _selectedSeals = [];
      if (this.iValue.length > 0) {
        this.iValue.map(async id => {
          if (id) {
            let response = await apiSeal.get(id);
            if (requestIsSuccess(response)) {
              _selectedSeals.push(response.data);
            }
          }
        });
      }
      this.selectedSeals = _selectedSeals;
    },

    selected(value) {
      this.selectedSeals = value;
      this.$emit(
        'change',
        this.selectedSeals[0] ? this.selectedSeals[0].id : null,
      );
 
    },

  },
  render() {
    let seals = this.tags.map(item => {
      return <div class="selected-item">
        <div class="selected-name"> {item ? item.name : null} </div>
        {!this.disabled ?
          <span
            class="btn-close"
            onClick={e => {
              e.stopPropagation();
              this.iValue = this.iValue.filter(id => id !== item.id);
              this.selectedSeals = this.selectedSeals.filter(_item => _item.id !== item.id);
              
              this.$emit(
                'change',
                this.iValue[0] ? this.iValue[0].id : null,
              );
              
            }}
          >
            <a-icon type="close" />
          </span> : undefined}
      </div>;
    });

    return (
      <div
        class={{
          'seal-select-panel': true,
          disabled: this.disabled,
          bordered: this.bordered,
        }}
        style={{
          height: this.bordered ? this.height + 'px' : 'auto',
        }}
        onClick={() => this.sealSelect()}
      >
        {this.tags.length == 0 ? <label class="tip">{this.placeholder}</label> : <div class="selected">{seals}</div>}

        {/* 文件选择模态框 */}
        <SmOaSealsSelectModal
          ref="SmOaSealsSelectModal"
          axios={this.axios}
          visible={this.iVisible}
          value={this.selectedSeals}
          onOk={this.selected}
          onChange={visible => (this.iVisible = visible)}
        />
      </div>
    );
  },
};
