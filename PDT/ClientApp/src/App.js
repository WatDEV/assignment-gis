import React, { Component } from 'react';
import { Router, Route, Switch } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';

import { history } from './helpers/history';

import { Page404, Page500 } from './views';
import DefaultLayout from "./layout/DefaultLayout";

// Import Font Awesome Icons Set
import 'font-awesome/css/font-awesome.min.css';

class App extends Component {

  render() {
    return (
      <React.Fragment>
        <ToastContainer position="top-right" autoClose={5000} style={{zIndex: 9999}}/>
        <Router history={history}>
          <Switch>
            <Route exact path="/404" name="Page 404" component={Page404} />
            <Route exact path="/500" name="Page 500" component={Page500} />
            <Route path="/" name="Home" component={DefaultLayout} />
          </Switch>
        </Router>
      </React.Fragment>
    );
  }
}

export default App;
