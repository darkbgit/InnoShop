import { Outlet } from "react-router";
import Header from "./Header/Header";

export default function RootLayout() {
  return (
    <div className="root-layout">
      <header>
        <Header />
      </header>

      <main>
        <Outlet />
      </main>

      <footer>
        <p>© 2025 Inno shop</p>
      </footer>
    </div>
  );
}
