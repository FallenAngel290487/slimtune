#ifndef VERSION_H
#define VERSION_H
#pragma once

#include <boost/preprocessor/stringize.hpp>

#define MAJOR_VERSION 0
#define MINOR_VERSION 2
#define REVISION_VERSION 0
#define BUILD_VERSION 0

#define VERSION_VALUE(x) x
#define VERSION_STRING_MACRO(a,b,c,d) BOOST_PP_STRINGIZE(a) "." BOOST_PP_STRINGIZE(b) "." \
	BOOST_PP_STRINGIZE(c) "." BOOST_PP_STRINGIZE(d)
#define VERSION_STRING VERSION_STRING_MACRO(MAJOR_VERSION, MINOR_VERSION, REVISION_VERSION, BUILD_VERSION)

#endif