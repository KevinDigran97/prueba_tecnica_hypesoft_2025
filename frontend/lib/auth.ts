import KeycloakProvider from "next-auth/providers/keycloak";
import { NextAuthOptions } from "next-auth";

export const authOptions: NextAuthOptions = {
  providers: [
    KeycloakProvider({
      clientId: process.env.KEYCLOAK_CLIENT_ID || process.env.NEXT_PUBLIC_KEYCLOAK_CLIENT_ID || "hypesoft-client",
      clientSecret: process.env.KEYCLOAK_CLIENT_SECRET || "", // vacío porque es público (frontend client)
      issuer: process.env.KEYCLOAK_ISSUER || `${process.env.NEXT_PUBLIC_KEYCLOAK_URL || "http://localhost:8080"}/realms/${process.env.NEXT_PUBLIC_KEYCLOAK_REALM || "hypesoft"}`,
    }),
  ],
  callbacks: {
    async jwt({ token, account, profile }) {
      // Persist the OAuth access_token and or the user id to the token right after signin
      if (account) {
        token.accessToken = account.access_token;
        token.refreshToken = account.refresh_token;
        token.idToken = account.id_token;
      }
      
      // Add user roles from Keycloak
      if (profile) {
        token.roles = (profile as any).realm_access?.roles || [];
        token.username = (profile as any).preferred_username;
        token.email = profile.email;
      }
      
      return token;
    },
    async session({ session, token }) {
      // Send properties to the client
      if (session.user) {
        (session.user as any).accessToken = token.accessToken;
        (session.user as any).roles = token.roles;
        (session.user as any).username = token.username;
      }
      return session;
    },
  },
  pages: {
    signIn: "/auth/login",
  },
  session: {
    strategy: "jwt",
  },
  secret: process.env.NEXTAUTH_SECRET || "your-secret-key-change-in-production",
};

