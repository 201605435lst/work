import SmMaterialPurchaseListSelectModal from './SmMaterialPurchaseListSelectModal';

export default {
  name: 'SmMaterialPurchaseListSelect',
  props: {
    axios: { type: Function, default: null },
    disabled: { type: Boolean, default: false },
    placeholder: { type: String, default: '请点击选择' },

  },
  data() {
    return {
      iVisible: false,//是否显示领料单模态框
      purchaseLists: [],
    };
  },
  methods: {
    //添加采购清单
    addMaterials(Val) {
      this.purchaseLists = Val;
      let Values = [], value_ = [], materials = [];
      Values = Val.map(item => {
        return [...item.purchaseListRltMaterials];
      });
      Values.forEach(e => {
        value_.push(...e);
      });
      for (let index = 0; index < value_.length; index++) {
        let isTrue = false;
        for (let index_ = index + 1; index_ < value_.length; index_++) {
          if (value_[index].id === value_[index_].id) {
            isTrue = true;
          }
        }
        isTrue ? '' : materials.push(value_[index]);
      }
      this.$emit('change', materials);
    },
  },
  render() {
    let Atag = [];
    for (let index in this.purchaseLists) {
      Atag.push(
        <a-tag style="margin: 2px">
          {this.purchaseLists[index].name}
        </a-tag>,
      );
    };
    return (
      <div class='SmMaterialPurchaseListSelect'>
        <a-col span={24} class="m-col tags" onClick={() => { this.iVisible = true; }}>
          <div class={Atag.length > 0 ? 'tags-div_' : 'tags-div'}>
            {Atag.length > 0 ? Atag : '请选择采购清单'}
          </div>
        </a-col>
        <SmMaterialPurchaseListSelectModal
          visible={this.iVisible}
          axios={this.axios}
          onChange={(Visible, Val) => {
            this.addMaterials(Val);
            this.iVisible = Visible;
          }}
        />
      </div>
    );
  },
};
