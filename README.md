# BlogCategory
Category manager for Content.

As you know, we have a "Content management" section in Nop Commerce.

But items cannot be categorized. In this plugin, we want to add this feature to Nop Commerce 4.4.
So we need a section for managing content categories to add, edit and delete categories. For this
section, you can get inspiration from product categories.

Fileds: Name, Tag (just normal text), Display order, parent category, picture


In Add or Edit Blog page we add a part for set categories. like product. (widget)


Api:

• Api for fetch category: this Api must have filter by Tag, Parent category Id, Parent category
Name.

• Blogs: get Blogs by category ids
➢ I have to point out that this must be implemented as a plugin, and there is no need to
change the core.

➢ Faces in backend side, front side you can complete after finish backend side
