import React, { Component } from 'react';
import { Redirect, Route, Switch } from 'react-router-dom';

// sidebar nav config
import navigation from '../_nav';

// routes config
import routes from '../_routes';

import { DefaultFooter, DefaultHeader } from './components';

class DefaultLayout extends Component {

  render() {

    return (
      <div className="app">
        <header>
          <DefaultHeader navItems={navigation.items}/>
        </header>
        <div className="app-body">
          <main className="main">
              <Switch>
                {routes.map((route, idx) => {
                    return route.component ? (<Route key={idx} path={route.path} exact={route.exact} name={route.name} render={props => (
                        <route.component {...props} />
                      )} />)
                      : (null);
                  },
                )}
                <Redirect from="/" to="/404" />
              </Switch>
          </main>
        </div>
        <footer>
          <DefaultFooter />
        </footer>
      </div>
    );
  }
}

export default DefaultLayout;
