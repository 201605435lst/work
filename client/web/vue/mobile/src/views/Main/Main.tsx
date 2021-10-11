import { defineComponent, ref } from "vue"

export default defineComponent({
  name: "Main",
  components: {},
  setup() {
    const active = ref(0)
    return { active }
  },

  render() {
    return (
      <div class="main">
        <router-view />
        
        <van-tabbar v-model={this.active}>
          <van-tabbar-item icon="home-o">标签</van-tabbar-item>
          <van-tabbar-item icon="search">标签</van-tabbar-item>
          <van-tabbar-item icon="friends-o">标签</van-tabbar-item>
          <van-tabbar-item icon="setting-o">标签</van-tabbar-item>
        </van-tabbar>
      </div>
    )
  }
})
