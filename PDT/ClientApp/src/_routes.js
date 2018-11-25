import React from "react";
import Loadable from "react-loadable";

function Loading() {
  return <div>Načítává se...</div>;
}

const Home = Loadable({
  loader: () => import("./views/Home/Home"),
  loading: Loading
});


const routes = [
  {
    path: "/",
    name: "Home",
    component: Home,
    exact: true
  },
];

export default routes;
