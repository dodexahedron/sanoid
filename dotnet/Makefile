SANOID_DOTNET_SOLUTION_ROOT ?= .

PROJECTFILE ?= $(SANOID_DOTNET_SOLUTION_ROOT)Sanoid/Sanoid.csproj

BUILDDIR ?= $(SANOID_DOTNET_SOLUTION_ROOT)/build
PUBLISHROOT ?= $(SANOID_DOTNET_SOLUTION_ROOT)/publish
RELEASECONFIG ?= Release-R2R
RELEASEDIR ?= $(BUILDDIR)/$(RELEASECONFIG)
RELEASEPUBLISHDIR ?= $(PUBLISHROOT)/$(RELEASECONFIG)
DEBUGCONFIG ?= Debug
DEBUGDIR ?= $(BUILDDIR)/$(DEBUGCONFIG)
DEBUGPUBLISHDIR ?= $(PUBLISHROOT)/$(DEBUGCONFIG)

PUBLISHBASECONFIGFILELIST = $(RELEASEPUBLISHDIR)/Sanoid.json $(RELEASEPUBLISHDIR)/Sanoid.nlog.json $(RELEASEPUBLISHDIR)/Sanoid.schema.json
PUBLISHBASECONFIGFILELIST += $(RELEASEPUBLISHDIR)/Sanoid.monitoring.schema.json $(RELEASEPUBLISHDIR)/Sanoid.local.schema.json

SANOIDDOCDIR ?= $(SANOID_DOTNET_SOLUTION_ROOT)/Documentation
MANDIR ?= /usr/local/man

LOCALSBINDIR ?= /usr/local/sbin
LOCALSHAREDIR ?= /usr/local/share
ETCDIR ?= /etc

SANOIDETCDIR ?= $(ETCDIR)/sanoid

all:	build-release

clean:  clean-all

clean-all:      clean-debug	clean-release

clean-debug:
	dotnet clean --configuration $(DEBUGCONFIG) -o $(DEBUGDIR) 2>/dev/null
	[ -d $(DEBUGDIR) ] && rm -rvf $(DEBUGDIR) || true
	rmdir -v $(BUILDDIR) || true
	rm -rfv Sanoid/bin/$(DEBUGCONFIG) 2>/dev/null
	rm -rfv Sanoid/obj/$(DEBUGCONFIG) 2>/dev/null
	rmdir -v Sanoid/bin || true
	rmdir -v Sanoid/obj || true
	rm -rfv Sanoid.Common/bin/$(DEBUGCONFIG) 2>/dev/null
	rm -rfv Sanoid.Common/obj/$(DEBUGCONFIG) 2>/dev/null
	rmdir -v Sanoid.Common/bin || true
	rmdir -v Sanoid.Common/obj || true
	rm -rfv Sanoid.Interop/bin/$(DEBUGCONFIG) 2>/dev/null
	rm -rfv Sanoid.Interop/obj/$(DEBUGCONFIG) 2>/dev/null
	rmdir -v Sanoid.Interop/bin || true
	rmdir -v Sanoid.Interop/obj || true
	rm -rfv Sanoid.Settings/bin/$(DEBUGCONFIG) 2>/dev/null
	rm -rfv Sanoid.Settings/obj/$(DEBUGCONFIG) 2>/dev/null
	rmdir -v Sanoid.Settings/bin || true
	rmdir -v Sanoid.Settings/obj || true

clean-release:
	dotnet clean --configuration $(RELEASECONFIG) -o $(RELEASEDIR) 2>/dev/null
	if [ -d $(RELEASEDIR) ] ; then rm -rvf $(RELEASEDIR) ; fi
	[ -d $(RELEASEPUBLISHDIR) ]  && rm -rvf $(RELEASEPUBLISHDIR) || true
	[ -d $(PUBLISHROOT) ]  && rm -rvf $(PUBLISHROOT) || true
	rmdir -v $(PUBLISHROOT) || true
	rmdir -v $(BUILDDIR) || true
	rm -rfv Sanoid/bin/$(RELEASECONFIG) 2>/dev/null
	rm -rfv Sanoid/obj/$(RELEASECONFIG) 2>/dev/null
	rmdir -v Sanoid/bin || true
	rmdir -v Sanoid/obj || true
	rm -rfv Sanoid.Common/bin/$(RELEASECONFIG) 2>/dev/null
	rm -rfv Sanoid.Common/obj/$(RELEASECONFIG) 2>/dev/null
	rmdir -v Sanoid.Common/bin || true
	rmdir -v Sanoid.Common/obj || true
	rm -rfv Sanoid.Interop/bin/$(RELEASECONFIG) 2>/dev/null
	rm -rfv Sanoid.Interop/obj/$(RELEASECONFIG) 2>/dev/null
	rmdir -v Sanoid.Interop/bin || true
	rmdir -v Sanoid.Interop/obj || true
	rm -rfv Sanoid.Settings/bin/$(RELEASECONFIG) 2>/dev/null
	rm -rfv Sanoid.Settings/obj/$(RELEASECONFIG) 2>/dev/null
	rmdir -v Sanoid.Settings/bin || true
	rmdir -v Sanoid.Settings/obj || true

extraclean:	clean-debug	clean-release
	rm -rfv Sanoid/bin 2>/dev/null
	rm -rfv Sanoid/obj 2>/dev/null
	rm -rfv Sanoid.Common/bin 2>/dev/null
	rm -rfv Sanoid.Common/obj 2>/dev/null
	rm -rfv Sanoid.Interop/bin 2>/dev/null
	rm -rfv Sanoid.Interop/obj 2>/dev/null
	rm -rfv Sanoid.Settings/bin 2>/dev/null
	rm -rfv Sanoid.Settings/obj 2>/dev/null


build:	build-release

build-debug:
	mkdir -p $(DEBUGDIR)
	dotnet build --configuration $(DEBUGCONFIG) -o $(DEBUGDIR)

build-release:
	mkdir -p $(RELEASEDIR)
	dotnet build --configuration $(RELEASECONFIG) -o $(RELEASEDIR) --use-current-runtime --no-self-contained -r linux-x64 Sanoid/Sanoid.csproj

reinstall:	uninstall	clean	install

install:	install-release	|	install-config	install-doc

install-config:	install-config-local	|	install-config-base

install-config-base:
	install --backup=existing -D -C -v -m 664 -t $(LOCALSHAREDIR)/Sanoid.net/ $(PUBLISHBASECONFIGFILELIST)

install-config-local:
	[ ! -d /etc/sanoid ] && [ -w /etc ] && mkdir -p /etc/sanoid || true
	[ ! /etc/sanoid/Sanoid.local.json ] && install --backup=existing -C -v -m 664 $(RELEASEPUBLISHDIR)/Sanoid.local.json $(SANOIDETCDIR)/Sanoid.local.json

install-config-local-force:
	[ ! -d /etc/sanoid ] && [ -w /etc ] && mkdir -p /etc/sanoid || true
	install --backup=existing -C -v -m 664 $(RELEASEPUBLISHDIR)/Sanoid.local.json $(SANOIDETCDIR)/Sanoid.local.json

install-doc:
	install -C -v -m 644 $(SANOIDDOCDIR)/Sanoid.8 $(MANDIR)/man8/Sanoid.8
	cp -fl $(MANDIR)/man8/Sanoid.8 $(MANDIR)/man8/Sanoid.net.8
	install -C -v -m 644 $(SANOIDDOCDIR)/Sanoid.5 $(MANDIR)/man5/Sanoid.5
	cp -fl $(MANDIR)/man5/Sanoid.5 $(MANDIR)/man5/Sanoid.net.5
	cp -fl $(MANDIR)/man5/Sanoid.5 $(MANDIR)/man5/Sanoid.json.5
	mandb -q

install-release:	publish-release
	install --backup=existing -C -v -m 750 $(RELEASEPUBLISHDIR)/Sanoid $(LOCALSBINDIR)/Sanoid
	cp -fs -v $(LOCALSBINDIR)/Sanoid $(LOCALSBINDIR)/Sanoid.net

publish-release:
	mkdir -p $(RELEASEPUBLISHDIR)
	dotnet publish --configuration $(RELEASECONFIG) --use-current-runtime --no-self-contained -r linux-x64 -p:PublishProfile=Linux-Release-R2R -o $(RELEASEPUBLISHDIR) Sanoid/Sanoid.csproj


uninstall:	uninstall-release	uninstall-config-base	uninstall-doc

uninstall-config-base:
	rm -fv $(LOCALSHAREDIR)/Sanoid.net/*.json 2>/dev/null

uninstall-config-local:
	rm -fv $(SANOIDETCDIR)/Sanoid.local.json 2>/dev/null
	rmdir -v $(SANOIDETCDIR) 2>/dev/null

uninstall-doc:
	rm -fv $(MANDIR)/man8/Sanoid.8 2>/dev/null
	rm -fv $(MANDIR)/man8/Sanoid.net.8 2>/dev/null
	rm -fv $(MANDIR)/man5/Sanoid.5 2>/dev/null
	rm -fv $(MANDIR)/man5/Sanoid.net.5 2>/dev/null
	mandb -q

uninstall-everything:	uninstall	uninstall-config-local

uninstall-release:
	rm -rfv $(LOCALSHAREDIR)/Sanoid.net 2>/dev/null
	rm -fv $(LOCALSBINDIR)/Sanoid 2>/dev/null
	tm -fv $(LOCALSBINDIR)/Sanoid.net 2>/dev/null

test:	test-everything

test-everything:
	dotnet test --settings=Sanoid.Common.Tests/everything.runsettings

test-everything-dangerous:
	dotnet test --settings=Sanoid.Common.Tests/everything-dangerous.runsettings

test-dangerous:
	dotnet test --settings=Sanoid.Common.Tests/dangerous.runsettings

save-sanoid-zfs-properties:
	@test ! -s $(SANOID_DOTNET_SOLUTION_ROOT)/propWipeUndoScript.sh || { echo Properties already saved. Will not overwrite. ; false ; }
	@echo "#!/bin/bash -x" >propWipeUndoScript.sh
	zfs get all -s local -rHo name,property,value | grep sanoid.net | while read obj prop val ; do echo zfs set $${prop}\=\"$${val}\" $${obj} >>propWipeUndoScript.sh ; done
	chmod 774 propWipeUndoScript.sh
	@echo Undo script saved as ./propWipeUndoScript
	@echo Run 'make restore-wiped-zfs-properties' or './propWipeUndoScript.sh' if you need to restore sanoid.net properties

wipe-sanoid-zfs-properties:
	@test -s $(SANOID_DOTNET_SOLUTION_ROOT)/propWipeUndoScript.sh || { echo Must save properties before wiping ; false ; }
	zfs get all -s local -rHo name,property | grep sanoid.net | while read obj prop ; do echo Removing $${prop} from $${obj} ; zfs inherit $${prop} $${obj} ; done
	$(info All properties removed)
	$(info Run make restore-wiped-zfs-properties to restore configuration)

restore-wiped-zfs-properties:
	@test -s $(SANOID_DOTNET_SOLUTION_ROOT)/propWipeUndoScript.sh || { echo No restore script. Did you forget to run make save-sanoid-zfs-properties\? ; false ; }
	./propWipeUndoScript.sh
	$(info Properties restored to the state they were in when you last ran make save-sanoid-zfs-properties)
