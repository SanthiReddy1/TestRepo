@SnapDeal
Feature: SnapDealSearch
	
@Search
Scenario: Add Products to Cart
	Given I login to the SnapDeal site
	When I search for 'Perfumes'
	And I apply 'Low To High' filter
	And I add No of products from the search results to cart
	Then I verify the products added to cart

