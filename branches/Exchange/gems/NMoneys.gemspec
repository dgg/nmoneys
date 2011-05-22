version = File.read(File.expand_path("../VERSION",__FILE__)).strip

Gem::Specification.new do |spec|
  spec.platform    = Gem::Platform::RUBY
  spec.name        = 'nmoneys'
  spec.version     = version
  spec.files = Dir['lib/**/*'] + Dir['docs/**/*']

  spec.summary     = 'NMoneys - .Net implementation of Money Value Object'
  spec.description = 'NMoneys is an implementation of the Money Value Object to support representing moneys in the currencies defined in the ISO 4217 standard'
  
  spec.authors           = ['Daniel Gonzalez Garcia']
  spec.email             = 'nmoneys@googlegroups.com'
  spec.homepage          = 'http://code.google.com/p/nmoneys/'
  spec.rubyforge_project = 'nmoneys'
end