<template>
  <div v-if="!processing">
    <v-card elevation="6" class="mx-auto" v-if="!processing">
      <v-card-text>
        <v-container fluid>
          <v-card-title class="justify-center"
            >Apps For Which You've Registered</v-card-title
          >
          <hr class="title-spacer" />
          <div class="app-buttons-scroll">
            <SelectAppButton
              v-for="(app, index) in registeredApps"
              :app="app"
              :key="index"
              :index="index"
              v-on:click.native="appAvailable(app) ? openUrl(app) : null"
            />
          </div>
        </v-container>
      </v-card-text>
    </v-card>
  </div>
</template>

<style scoped>
.app-buttons-scroll {
  display: flex;
  overflow-x: auto;
}
</style>

<script>
import { mapActions } from "vuex";
import { mapGetters } from "vuex";
import SelectAppButton from "@/components/widgets/SelectAppButton";
import { appProvider } from "@/providers/appProvider";
import User from "@/models/user";

export default {
  name: "MyRegisteredAppsWidget",
  components: {
    SelectAppButton,
  },
  data: () => ({
    user: new User(),
    registeredApps: [],
    processing: false
  }),
  methods: {
    ...mapActions("appModule", [ "updateRegisteredApps" ]),
    
    appAvailable(app) {
      console.log("appAvailable invoked...");
      if (app.isActive) {
        if (!app.inDevelopment) {
          if (app.liveUrl !== "") {
            return true;
          } else {
            return false;
          }
        }
      } else {
        return false;
      }
    },

    openUrl(app) {
      console.log("openUrl invoked...");
      window.open(app.liveUrl, "_blank");
    },
  },
  computed: {
    ...mapGetters("settingsModule", ["getUser"]),
    ...mapGetters("appModule", [ "getRegisteredApps" ]),
  },
  async created() {
    this.$data.processing = true;
    this.$data.user = new User(this.getUser);

    const storeRegisteredApps = this.getRegisteredApps;

    if (storeRegisteredApps.length === 0) {
      const response = await appProvider.getRegisteredApps(this.$data.user.id);

      if (response.success) {
        this.updateRegisteredApps(response.apps);
      }
    }

    storeRegisteredApps.forEach((store) => {
      this.$data.registeredApps.push(store);
    });

    this.$data.processing = false;
  },
}
</script>