import React, { Component } from "react";

import AuthService from "../services/auth.services";

import { Link, useMatch, useResolvedPath } from "react-router-dom";

export default class NavbarNew extends Component {
  constructor(props) {
    super(props);
    this.logOut = this.logOut.bind(this);

    this.state = {
      isStaff: false,
      isAdmin: false,
      currentUser: undefined,
    };
  }

  componentDidMount() {
    const user = AuthService.getCurrentUser();

    console.log(user);
    if (user) {
      this.setState({
        currentUser: user,
        isAdmin: user.Role.includes("ROLE_ADMIN"),
        isStaff: user.Role.includes("ROLE_STAFF"),
      });
    }
  }

  logOut() {
    const confirm = window.confirm("Are you sure you want to log out?");
    if (confirm) {
      AuthService.logout();
      this.setState({
        isStaff: false,
        isAdmin: false,
        currentUser: undefined,
      });
      window.location = "/";
    }
  }

  render() {
    const { currentUser, isAdmin, isStaff } = this.state;
    return (
      <nav className="nav">
        <Link to="/" className="site-title">
          RateGame
        </Link>
        <ul>
          {currentUser && <CustomLink href="/profile">Profile</CustomLink>}
          {!currentUser && <CustomLink href="/login">Login</CustomLink>}
          {!currentUser && <CustomLink href="/register">Register</CustomLink>}
          {isAdmin && <CustomLink href="/review/new">New review</CustomLink>}
          {isStaff && <CustomLink href="/review/new">New review</CustomLink>}

          {currentUser && (
            <Link onClick={this.logOut} className="logout-link">
              Log out
            </Link>
          )}
        </ul>
      </nav>
    );
  }
}

function CustomLink({ href, children, ...props }) {
  const resolvedPath = useResolvedPath(href);
  const isActive = useMatch({ path: resolvedPath.pathname, end: true });
  return (
    <li className={isActive ? "active" : ""}>
      <Link to={href} {...props}>
        {children}
      </Link>
    </li>
  );
}
