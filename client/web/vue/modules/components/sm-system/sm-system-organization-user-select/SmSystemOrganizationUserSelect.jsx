import OrganizationSelect from '../sm-system-organization-tree-select';
import UserSelect from '../sm-system-user-select';

export default {
  name: 'SmSystemOrganizationUserSelect',
  model: ['input', 'change'],
  props: {
    organizationId: { type: String, default: null },
    axios: { type: Function, default: null },
    value: {},
    disabled: { type: Boolean, default: false },
    mode: { type: String, default: 'default' },
  },
  data() {
    return {
      iOrganizationId: null,
      iValue: null,
    };
  },
  computed: {},
  watch: {
    organizationId: {
      handler: function (val, oldVal) {
        this.iOrganizationId = val;
      },
      immediate: true,
    },
    value: {
      handler: function (val, oldVal) {
        if ((val === null || val === undefined) && this.mode === 'multiple') {
          this.iValue = [];
        } else {
          this.iValue = val;
        }
      },
      immediate: true,
    },
  },

  async created() { },

  methods: {},

  render() {
    return (
      <a-input-group compact>
        <OrganizationSelect
          axios={this.axios}
          disabled={this.disabled}
          value={this.iOrganizationId}
          onInput={value => {
            this.iOrganizationId = value;
            this.iValue = this.mode === 'default' ? null : [];
            this.$emit('orgInput', value);
            this.$emit('orgChange', value);
          }}
          style="width: 35%"
        />
        <UserSelect
          axios={this.axios}
          mode={this.mode}
          disabled={this.disabled}
          organizationId={this.iOrganizationId}
          value={this.iValue}
          onInput={value => {
            this.iValue = value;
            this.$emit('input', value, this.iOrganizationId);
            this.$emit('change', value, this.iOrganizationId);
          }}
          style="width: 65%"
        />
      </a-input-group>
    );
  },
};
