Feature: Create blog
	In order to provide a new blog
	I want to be able to create a blog

Background:
	Given A configured environment

Scenario: Create a blog should be ok
	When I create a new blog : 'http://newblog.io'
	Then the blog 'http://newblog.io' must be created