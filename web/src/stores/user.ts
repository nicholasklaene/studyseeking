import axios from "axios";
import { defineStore } from "pinia";
import { getApiUrl } from "@/utils";
import { useAuthStore } from "./auth";

const baseUrl = getApiUrl();

export const useUserStore = defineStore("user", {
  actions: {
    async createUser() {
      const authStore = useAuthStore();

      let returningUsersLocal = localStorage.getItem("returningUsers");
      if (!returningUsersLocal) returningUsersLocal = "[]";
      const returningUsersList: string[] = JSON.parse(returningUsersLocal);

      if (returningUsersList.includes(authStore.state.username)) return;

      const response = await axios.post(`${baseUrl}/users`);
      if (response.status === 201) {
        returningUsersList.push(authStore.state.username);
        localStorage.setItem(
          "returningUsers",
          JSON.stringify(returningUsersList)
        );
      }
    },
  },
});
