import Navbar from "./components/NavbarNew";
import "./App.css";
import Profile from "./pages/Profile";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Home from "./pages/Home";
import Review from "./pages/Review";
import { Route, Routes } from "react-router-dom";
import NewReview from "./pages/NewReview";

import EditReview from "./pages/EditReview";

import NotFound from "./pages/NotFound";

function App() {
  return (
    <>
      <Navbar />
      <Routes>
        <Route path="/" element={<Home />}></Route>
        <Route path="/Profile" element={<Profile />}></Route>
        <Route path="/Login" element={<Login />}></Route>
        <Route path="/Register" element={<Register />}></Route>
        <Route path="/review/:id" element={<Review />}></Route>
        <Route path="/review/new" element={<NewReview />}></Route>
        <Route path="/review/edit/:id" element={<EditReview />}></Route>
        <Route path="*" element={<NotFound />}></Route>
      </Routes>
    </>
  );
}

export default App;
