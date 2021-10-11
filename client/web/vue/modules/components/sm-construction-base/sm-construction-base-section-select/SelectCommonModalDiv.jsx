import './style';
import ApiSection from '../../sm-api/sm-construction-base/ApiSection';

let apiSection = new ApiSection();

// 选择公用的Modal(模态框)div
export default {
  name: 'SelectCommonModalDiv',
  props: {
    tags: { type: Array, default: ()=>[] }, // tag列表
  },
  data() {
    return {

    };
  },
  computed: {},
  watch: {},
  async created() {

  },
  methods: {},
  render() {
    return (
      <div class="sm-basic-selected-modal">
        <div class='selected'>
          {this.tags && this.tags.length > 0 ? (
            this.tags.map(item => {
              return <div class='selected-item'>
                <a-icon style={{ color: '#f4222d' }} type={'carry-out'} />
                <span class='selected-name'> {item ? item.name : null} </span>
                <span
                  class='btn-close'
                  onClick={() => {
                    this.$emit('delClick', item);
                  }}
                >
                  <a-icon type='close' />
                </span>
              </div>;
            })

          ) : (
            <span style='margin-left:10px;'>请选择</span>
          )}
        </div>
      </div>
    );
  },
};
