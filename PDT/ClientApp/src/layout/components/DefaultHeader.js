import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { Link } from "react-router-dom";

class DefaultHeader extends Component {

  render() {
    return (
      <div>
        <ul>
          {this.props.navItems.map((item, i) => (
            <li key={i}>
              <Link to={item.url}>
                <i className={item.icon} style={{marginRight: "5px"}}/>
                {item.name}
              </Link>
            </li>
          ))}
        </ul>
      </div>
    );
  }
}

DefaultHeader.propTypes = {
  navItems: PropTypes.array.isRequired,
};

export default DefaultHeader;
